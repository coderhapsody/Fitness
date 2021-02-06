Imports System
Imports System.Text
Imports System.Web
Imports System.Drawing
Imports System.Web.SessionState
Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Imports System.Linq
Imports System.Xml.Linq
Imports ImageManipulation

Namespace ImageHandlers
    Public Class ResizeImages
        Implements IHttpHandler

        Enum ImageType As Integer
            JPG
            PNG
            GIF
        End Enum

        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

        Dim ResizedImagesDirectory As String = ConfigurationManager.AppSettings("ResizedImagesDirectory")

        Public Sub ProcessRequest(ByVal Ctx As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest

            'Get the current request
            Dim Req As HttpRequest = Ctx.Request


            'Set the width and height for the resized image
            Dim Width As Integer = 0
            Dim Height As Integer = 0
            Dim Key As String = ""
            If Not IsNothing(Req.QueryString("w")) And IsNumeric(Req.QueryString("w")) Then
                Width = Req.QueryString("w")
            End If
            If Not IsNothing(Req.QueryString("h")) And IsNumeric(Req.QueryString("h")) Then
                Height = Req.QueryString("h")
            End If
            If Not IsNothing(Req.QueryString("k")) Then
                Key = Req.QueryString("k")
            End If

            'If we have a key stored in an xml file, use it to determine the width and height of the image instead
            If Key.Length > 0 Then
                Dim KeyImage As New ResizedImage
                KeyImage = GetImageDimensions(Ctx, Key)
                Height = KeyImage.Height
                Width = KeyImage.Width
            End If

            Dim DisplayResizedImage As Boolean = True
            If Width = 0 And Height = 0 Then
                'They didn't set a height or width, so don't create or display a resized image
                'Use the original image instead
                DisplayResizedImage = False
            End If

            'Get the path of the file, without the .ashx extension
            Dim PhysicalPath As String = Regex.Replace(Req.PhysicalPath, "\.ashx.*", "")

            'Determine the content type, and save what image type we have for later use
            Dim ImgType As ImageType
            If PhysicalPath.EndsWith(".jpg") Or PhysicalPath.EndsWith(".jpeg") Then
                Ctx.Response.ContentType = "image/jpeg"
                ImgType = ImageType.JPG
            ElseIf PhysicalPath.EndsWith(".gif") Then
                Ctx.Response.ContentType = "image/gif"
                ImgType = ImageType.GIF
            ElseIf PhysicalPath.EndsWith(".png") Then
                Ctx.Response.ContentType = "image/png"
                ImgType = ImageType.PNG
            End If



            Try
                If DisplayResizedImage Then 'display resized image

                    'Name the images based on their width, height, and path to ensure they're unique.
                    'The image name starts out looking like /HttpModule/images/turtle.jpg (the virtual path), and gets
                    'converted to 400_200_images_turtle.jpg.  The 400 is the width, and the 200 is the height.
                    'If a width or height is not specified, it will look like 0_200_images_turtle.jpg (an example
                    'where the width is not specified).           
                    Dim VirtualPath As String = Regex.Replace(Req.Path, "\.ashx.*", "")
                    Dim ResizedImageName As String = Regex.Replace(VirtualPath, "/", "_")
                    ResizedImageName = Regex.Replace(ResizedImageName, "_.*?_", "")
                    ResizedImageName = Width & "_" & Height & "_" & ResizedImageName

                    'Get the resized image
                    Dim ri As New ResizedImage
                    ri = GetResizedImage(Ctx, ResizedImageName, Height, Width, ImgType)

                    'And display it
                    Ctx.Response.WriteFile(Path.Combine(ri.ImagePath, ri.ImageName))
                Else
                    'display original image
                    Ctx.Response.WriteFile(PhysicalPath)
                End If

            Catch ex As Exception
                'You can add logging here if you want, but most like the image path can't be found,
                'so don't do anything
            End Try

        End Sub

        Private Sub Cleanup(ByVal Ctx As System.Web.HttpContext)
            'Delete images in your resized directory once more than the max number are saved there.  This will prevent 
            'the resized images on your site from taking up too much hard drive space
            Dim CleanupEnabled As Boolean = CType(ConfigurationManager.AppSettings("CleanupEnabled"), Boolean)

            If CleanupEnabled Then

                Dim MaxResizedImages As Integer = ConfigurationManager.AppSettings("MaxResizedImages")
                Dim NumDeleteWhenOverMax As Integer = ConfigurationManager.AppSettings("NumDeleteWhenOverMax")

                'Get the cached list of resized images
                Dim ResizedImageList As New List(Of ResizedImage)
                ResizedImageList = Ctx.Cache.Get("ResizedImageList")
                
                If Not IsNothing(ResizedImageList) Then
                    If ResizedImageList.Count > MaxResizedImages Then

                        'Default sort by date (oldest come first in the list)
                        ResizedImageList.Sort()


                        'Delete the specified number of images from the resized images folder, 
                        'starting with the oldest images
                        For i As Integer = 0 To NumDeleteWhenOverMax - 1
                            Dim ri As ResizedImage = ResizedImageList(i)
                            Dim FullPath As String = Path.Combine(ri.ImagePath, ri.ImageName)
                            Try
                                File.Delete(FullPath)
                            Catch ex As Exception
                                'There's a chance more than one process will be trying to delete the same
                                'file at the same time, so catch any exceptions here just in case
                            End Try
                        Next

                        'Cache is automatically cleared because there is a cache dependency on this folder
                    End If
                End If

            End If

        End Sub

        Private Function GetResizedImage(ByVal Ctx As System.Web.HttpContext, ByVal ImageName As String, ByVal Height As Integer, _
                                        ByVal Width As Integer, ByVal ImgType As ImageType) As ResizedImage




            'Look in the cache first for a list of images that have been resized
            Dim ResizedImageList As New List(Of ResizedImage)
            Dim ResizedImage As New ResizedImage

            ResizedImageList = Ctx.Cache.Get("ResizedImageList")
            If IsNothing(ResizedImageList) Then
                ResizedImageList = New List(Of ResizedImage)
            End If


            'Create the folder where we want to save the resized images if it's not already there
            Dim ResizedImagePath As String = Ctx.Server.MapPath(ResizedImagesDirectory)
            If Not Directory.Exists(ResizedImagePath) Then
                Directory.CreateDirectory(ResizedImagePath)
            End If

            If Not ImageInList(ImageName, Width, Height, ResizedImageList) Then
                'We didn't find the image in the list of resized images...look in the resized folder
                'and see if it's there
                Dim ImageFullPath As String = Path.Combine(ResizedImagePath, ImageName)
                If File.Exists(ImageFullPath) Then
                    'It's already there, no need to resize it, just rebuild the cached list of images
                    RebuildCache(Ctx)
                Else

                    'Before creating a new resized image, clear out the resized images directory if more than 
                    'the max allowed number of images are in there.
                    Cleanup(Ctx)

                    'If the image doesn't exist, we need to resize it 
                    Dim Req As HttpRequest = Ctx.Request
                    Dim PhysicalPath As String = Regex.Replace(Req.PhysicalPath, "\.ashx.*", "")
                    ResizeImage(PhysicalPath, ResizedImagePath, ImageName, Width, Height, ImgType)

                    'Now update the cache
                    RebuildCache(Ctx)
                End If
            End If


            'Let's set the properties of the image to return
            ResizedImage.ImageName = ImageName
            ResizedImage.ImagePath = ResizedImagePath
            ResizedImage.Height = Height
            ResizedImage.Width = Width

            Return ResizedImage
        End Function

        Private Function ImageInList(ByVal ImageName As String, ByVal Width As Integer, _
                                      ByVal Height As Integer, ByVal ResizedImageList As List(Of ResizedImage)) As Boolean
            'Let's see if an image with this name and size is already created
            For Each ri As ResizedImage In ResizedImageList
                If ri.ImageName = ImageName And ri.Height = Height _
                    And ri.Width = Width Then
                    Return True
                    Exit For
                End If
            Next

            Return False
        End Function

        Private Sub RebuildCache(ByVal ctx As HttpContext)
            'Clear the cache
            ctx.Cache.Remove("ResizedImageList")

            'Now loop through the resized images folder and re-add every item in it to our list of
            'resized images, then cache that list.  This way, for most reads we don't have to look
            'at the actual physical directory to determine if a file has been resized or not, we
            'can just look at the cache instead
            Dim ResizedImageList As New List(Of ResizedImage)
            Dim ResizedImagePath As String = ctx.Server.MapPath(ResizedImagesDirectory)
            Dim di As New DirectoryInfo(ResizedImagePath)
            For Each fi As FileInfo In di.GetFiles()

                If fi.Extension.ToLower = ".jpeg" Or fi.Extension.ToLower = ".jpg" _
                    Or fi.Extension.ToLower = ".gif" Or fi.Extension.ToLower = ".png" Then

                    'Get the width and height (stored in the image name, 
                    'width first then height, like 800_600_images_turtle.jpg)
                    Dim WidthAndHeight() As String = fi.Name.Split("_")
                    Dim Width As Integer = 0
                    Dim Height As Integer = 0

                    If WidthAndHeight.Length >= 2 Then 'make sure we have a width and height
                        If IsNumeric(WidthAndHeight(0)) Then
                            Width = WidthAndHeight(0)
                        End If
                        If IsNumeric(WidthAndHeight(1)) Then
                            Height = WidthAndHeight(1)
                        End If
                    End If

                    'Add the image to the list
                    Dim ri As New ResizedImage
                    ri.ImageName = fi.Name
                    ri.ImagePath = ResizedImagePath
                    ri.Height = Height
                    ri.Width = Width
                    ri.DateModified = fi.LastWriteTime
                    ResizedImageList.Add(ri)

                End If
            Next

            'Keep the cache for a day, unless new images get added to or deleted from
            'the resized image folder
            Dim cd As New CacheDependency(ResizedImagePath)
            Dim ts As New TimeSpan(24, 0, 0)
            ctx.Cache.Add("ResizedImageList", ResizedImageList, cd, _
                          Cache.NoAbsoluteExpiration, ts, CacheItemPriority.Default, Nothing)

        End Sub

        Private Sub ResizeImage(ByVal ImagePath As String, ByVal ResizedSavePath As String, ByVal ResizedImageName As String, _
                                ByVal NewWidth As Integer, ByVal NewHeight As Integer, ByVal ImgType As ImageType)

            'Make sure the image exists before trying to resize it
            If File.Exists(ImagePath) And Not (NewHeight = 0 And NewWidth = 0) Then
                Using OriginalImage As New Bitmap(ImagePath)

                    If NewWidth > 0 And NewHeight = 0 Then
                        'The user only set the width, calculate the new height
                        NewHeight = Math.Floor(OriginalImage.Height / (OriginalImage.Width / NewWidth))
                    End If

                    If NewHeight > 0 And NewWidth = 0 Then
                        'The user only set the height, calculate the width
                        NewWidth = Math.Floor(OriginalImage.Width / (OriginalImage.Height / NewHeight))
                    End If

                    If NewHeight > OriginalImage.Height Or NewWidth > OriginalImage.Width Then
                        'Keep the original height and width to avoid losing image quality
                        NewHeight = OriginalImage.Height
                        NewWidth = OriginalImage.Width
                    End If


                    Using ResizedImage As New Bitmap(OriginalImage, NewWidth, NewHeight)

                        'For jpg and png files, use the normal gdi image resizing
                        Dim newGraphic As Graphics = Graphics.FromImage(ResizedImage)
                        Select Case ImgType
                            Case ImageType.JPG, ImageType.PNG
                                ResizedImage.SetResolution(72, 72)
                                newGraphic.Clear(Color.White)
                                newGraphic.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                                newGraphic.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                        End Select

                        'Resize the image for all image types
                        newGraphic.DrawImage(OriginalImage, 0, 0, NewWidth, NewHeight)


                        'Save the image as the appropriate type
                        Dim FullSavePath As String = System.IO.Path.Combine(ResizedSavePath, ResizedImageName)
                        Select Case ImgType
                            Case ImageType.GIF
                                'http://msdn.microsoft.com/en-us/library/aa479306.aspx
                                'Use octree quantization algorithm for a better result for .gif files
                                Dim UseOctreeQuantization As Boolean = CType(ConfigurationManager.AppSettings("UseOctreeQuantizationForGIF"), Boolean)
                                If UseOctreeQuantization Then
                                    SaveOctreeQuantizedGIF(ResizedImage, NewWidth, NewHeight, FullSavePath)
                                Else
                                    ResizedImage.Save(FullSavePath, Imaging.ImageFormat.Gif)
                                End If
                            Case ImageType.JPG
                                ResizedImage.Save(FullSavePath, Imaging.ImageFormat.Jpeg)
                            Case ImageType.PNG
                                ResizedImage.Save(FullSavePath, Imaging.ImageFormat.Png)
                        End Select

                    End Using
                End Using
            End If
        End Sub


        Private Sub SaveOctreeQuantizedGIF(ByVal ResizedImage As Bitmap, ByVal NewWidth As Integer, ByVal NewHeight As Integer, ByVal SavePath As String)
            'Use octree quantization algorithm for a better resized image for .gif files
            Using ResizedImage
                Dim Quantizer As New OctreeQuantizer(255, 8)
                Using Quantized As Bitmap = Quantizer.Quantize(ResizedImage)
                    Quantized.Save(SavePath, Imaging.ImageFormat.Gif)
                End Using
            End Using
        End Sub

        Private Function GetImageDimensions(ByVal Ctx As HttpContext, ByVal Key As String) As ResizedImage

            Dim ri As New ResizedImage

            'If I enter the key "thumbnail", this function will go to the xml file to find out
            'what the width and height of a thumbnail should be.

            'The xml that we're reading from looks like this.  You can set the width, the height, or both
            '<ResizedImages>
            '  <image name="thumbnail" width="100" height="100" />
            '  <image name="normal" width="200" />  
            '  <image name="large" height="300" />  
            '</ResizedImages>

            'Load the xml file
            Dim XMLSource As XElement = GetResizedImageKeys(Ctx)

            'Get all nodes where the name equals the key
            'To make this code work in .Net 2.0, use an xpath query to get the height
            'and width values instead of a LINQ query
            Dim ResizedQuery = From r In XMLSource.Elements("image") _
                            Where r.Attribute("name") = Key _
                            Select r

            'Set the resized image we're returning with the width and height
            For Each r As XElement In ResizedQuery
                If Not IsNothing(r.Attribute("height")) Then
                    ri.Height = r.Attribute("height")
                End If
                If Not IsNothing(r.Attribute("width")) Then
                    ri.Width = r.Attribute("width")
                End If
            Next

            Return ri
        End Function

        Private Function GetResizedImageKeys(ByVal ctx As HttpContext) As XElement
            Dim xel As XElement = Nothing
            Dim ResizedImageKeys As String = ctx.Server.MapPath(ConfigurationManager.AppSettings("ResizedImageKeys"))
            If Not IsNothing(ResizedImageKeys) Then
                'Try to get the xml from the cache first
                xel = ctx.Cache.Get("ResizedImageKeys")

                'If it's not there, load the xml document and then add it to the cache
                If IsNothing(xel) Then
                    xel = XElement.Load(ResizedImageKeys)
                    Dim cd As New CacheDependency(ResizedImageKeys)
                    Dim ts As New TimeSpan(24, 0, 0)
                    ctx.Cache.Add("ResizedImageKeys", xel, cd, _
                                  Cache.NoAbsoluteExpiration, ts, CacheItemPriority.Default, Nothing)
                End If
            End If
            Return xel
        End Function




        'This class is used to keep track of which images are resized.  We save this in a cached list and look here first,
        'so we don't have to look through the folder on the file system every time we want to see if the resized image
        'exists or not
        Private Class ResizedImage
            Implements IComparable(Of ResizedImage)
            Private _ImageName As String
            Private _ImagePath As String
            Private _Width As Integer
            Private _Height As Integer
            Private _DateModified As DateTime
            Public Property DateModified() As DateTime
                Get
                    Return _DateModified
                End Get
                Set(ByVal value As DateTime)
                    _DateModified = value
                End Set
            End Property
            Public Property ImageName() As String
                Get
                    Return _ImageName
                End Get
                Set(ByVal value As String)
                    _ImageName = value
                End Set
            End Property
            Public Property ImagePath() As String
                Get
                    Return _ImagePath
                End Get
                Set(ByVal value As String)
                    _ImagePath = value
                End Set
            End Property
            Public Property Width() As Integer
                Get
                    Return _Width
                End Get
                Set(ByVal value As Integer)
                    _Width = value
                End Set
            End Property
            Public Property Height() As Integer
                Get
                    Return _Height
                End Get
                Set(ByVal value As Integer)
                    _Height = value
                End Set
            End Property
            Public Sub New()
                Width = 0
                Height = 0
                ImagePath = ""
                ImageName = ""
                DateModified = DateTime.Now()
            End Sub

            'Default sort by date modified (oldest date modified will come first in the list when we sort)
            Public Function CompareTo(ByVal Other As ResizedImage) As Integer _
                Implements IComparable(Of ImageHandlers.ResizeImages.ResizedImage).CompareTo
                Return DateModified.CompareTo(Other.DateModified)
            End Function
        End Class
    End Class

End Namespace
