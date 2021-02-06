Option Explicit
On Error Resume Next

'======================================================================
'
' Utility routines
'
'======================================================================




'---------------------------------------
' Error handling functions



Dim g_strErrorContext, g_nSavedErrorNumber, g_strSavedErrorDescription, g_strSavedErrorSource
   
Sub ClearCurrentErrorContext
    On Error Resume Next ' This does an Err.Clear for us.

    g_strErrorContext = Empty
    g_nSavedErrorNumber = Empty
    g_strSavedErrorDescription = Empty
    g_strSavedErrorSource = Empty
End Sub

' If we are getting called a second time after having already saved off the original error, we will not
' resave the current error context because if may be a new error. We will however append the new context
' message to the existing context message so that a "call-stack" can be constructed.
Sub SaveCurrentErrorContext(strContextMessage)
    ' Not using On Error Resume Next because it kills the active error context.

    If IsEmpty(g_nSavedErrorNumber) And (Err.Number <> 0) Then
        g_nSavedErrorNumber = Err.Number
        g_strSavedErrorDescription = Err.Description
        g_strSavedErrorSource = Err.Source
    End If
    If Not IsEmpty(strContextMessage) Then g_strErrorContext = g_strErrorContext & vbCRLF & strContextMessage
End Sub


Function IIF(fBooleanResult, varTrue, varFalse)
	If fBooleanResult Then
		If IsObject(varTrue) Then
			Set IIF = varTrue
		Else
			IIF = varTrue
		End If
	Else
		If IsObject(varFalse) Then
			Set IIF = varFalse
		Else
			IIF = varFalse
		End If
	End If
End Function








' Instantiate the timer objects that we use for timing various operations.
Dim g_tmrPage, g_tmrStep, g_tmrLoop, g_csecPage, g_csecStep, g_csecLoop
Set g_tmrPage = CreateObject("BillGrHighResTimer.BillGrHighResTimer")
Set g_tmrStep = CreateObject("BillGrHighResTimer.BillGrHighResTimer")
Set g_tmrLoop = CreateObject("BillGrHighResTimer.BillGrHighResTimer")
g_tmrPage.Restart
Err.Clear ' Ignore any errors if this COM object is not installed.

Dim g_strStatisticLoggingContext, g_rgstrLoggedStatistics, g_iLastValidLoggedStatistic

Sub LogStatistic(strDescription, nValue)
    On Error Resume Next

    Dim strTemp

    If Len(g_strStatisticLoggingContext) > 0 Then
        strTemp = g_strStatisticLoggingContext & ": "
    End If

    strTemp = strTemp & strDescription & " = " & FormatNumber(nValue, 2)

	Dim sizeNew, iNewElement
	If IsArray(g_rgstrLoggedStatistics) Then
		' See if the array is full. Expand it if that's the case.
		If g_iLastValidLoggedStatistic = UBound(g_rgstrLoggedStatistics) Then
			sizeNew = UBound(g_rgstrLoggedStatistics) * 2
			ReDim Preserve g_rgstrLoggedStatistics(sizeNew)
		End If
	Else
		' We've never been called before so we have to allocate the initial array.
		g_iLastValidLoggedStatistic = -1
		sizeNew = 16
		ReDim g_rgstrLoggedStatistics(sizeNew)
	End If

	g_iLastValidLoggedStatistic = g_iLastValidLoggedStatistic + 1
	g_rgstrLoggedStatistics(g_iLastValidLoggedStatistic) = strTemp

    Err.Clear
End Sub

Function GetLoggedStatistics()
	On Error Resume Next

	Dim strRet, strKeyName, strValue, iEntry
	For iEntry = LBound(g_rgstrLoggedStatistics) To UBound(g_rgstrLoggedStatistics)
		If Not IsEmpty(g_rgstrLoggedStatistics(iEntry)) Then

			strRet = strRet & g_rgstrLoggedStatistics(iEntry) & vbCRLF

		End If
	Next

	GetLoggedStatistics = strRet
End Function












' NOTE: Not using FUNCSUB_PROLOG because there will always be an active incoming error.
Sub DisplayErrorSummaryAndExit(strEndUserContextMessage)
'	On Error Resume Next

    If Err.Number = 0 Then
        ' There is not an active error. We need to raise one so that the end-user context message
        ' can be logged correctly.
        Err.Raise 1, "DisplayErrorSummaryAndExit()", "strEndUserContextMessage = " & strEndUserContextMessage
        SaveCurrentErrorContext strEndUserContextMessage
    End If

	' Append the end-user context message to the current error context before logging.
    SaveCurrentErrorContext strEndUserContextMessage

    ' Display the error context.
    LogCurrentErrorContext "Error", strEndUserContextMessage
	wscript.Echo
	wscript.Echo strEndUserContextMessage
	wscript.Echo
	wscript.Echo "Error Number: " & g_nSavedErrorNumber
	wscript.Echo "Error Description: " & g_strSavedErrorDescription
	wscript.Echo "Error Source: " & g_strSavedErrorSource
	wscript.Echo
	wscript.Quit g_nSavedErrorNumber
End Sub



'======================================================================
'
' File I/O functions
'
'======================================================================



' Scripting.FileSystemObject "iomode" argument
Const ForReading = 1
Const ForWriting = 2
Const ForAppending = 8

' Scripting.FileSystemObject "format" argument
Const tsASCII = 0
Const tsUnicode = -1



'----------------------------------------
'
'   GetFileSystemObject
'
'   Description:
'
'       This function will create and cache a Scripting.FileSystem object
'		for this page.
'
'	Return value:
'
'		If this function performed correctly, the return value will be a reference
'		to the globa Scripting.FileSystem object for this page. Otherwise, the
'		return value will be Nothing.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Dim g_fsoPerPage

Function GetFileSystemObject()
	On Error Resume Next

	' Prep for error exit.
	Set GetFileSystemObject = Nothing

	If IsEmpty(g_fsoPerPage) Then
		Set g_fsoPerPage = CreateObject("Scripting.FileSystemObject")
		If Err.Number <> 0 Then SaveCurrentErrorContext "GetFileSystemObject() - Error instantiating FileSystemObject." : Exit Function
	End If
	
	Set GetFileSystemObject = g_fsoPerPage
End Function




'----------------------------------------
'
'   BindTextStreamToFile
'
'   Parameters:
'
'       strFileName
'           The name of the file to be opened.
'
'   Description:
'
'       This function will bind a TextStream object to the specified file. The file
'		will be examined first and the TextStream will be opened in the correct
'		encoding mode (ASCII or Unicode).
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Function BindTextStreamToFile(strFileName)
	On Error Resume Next

	' Prep for error exit.
	Set BindTextStreamToFile = Nothing

	' Determine the encoding type of the file.
    Dim tsAsciiUnicodeMode
    If IsUnicodeFile(strFileName) Then
        tsAsciiUnicodeMode = tsUnicode
    Else
        tsAsciiUnicodeMode = tsAscii
    End If
    If Err.Number <> 0 Then SaveCurrentErrorContext "BindTextStreamToFile() - Error determining encoding format of file: " & strFileName : Exit Function

	' Bind a stream to the file using the correct encoding format.
	Set BindTextStreamToFile = BindTextStreamToFileEx(strFileName, tsAsciiUnicodeMode)
    If Err.Number <> 0 Then SaveCurrentErrorContext "BindTextStreamToFile() - Error binding stream to file: " & strFileName : Exit Function

End Function



'----------------------------------------
'
'   BindTextStreamToFileEx
'
'   Parameters:
'
'       strFileName
'           The name of the file to be opened.
'
'		tsAsciiUnicodeMode
'			Specifies the mode to use when opening the file.
'
'   Description:
'
'       This function will bind a TextStream object to the specified file using
'		the specified encoding mode.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Function BindTextStreamToFileEx(strFileName, tsAsciiUnicodeMode)
	On Error Resume Next

	' Prep for error exit.
	Set BindTextStreamToFileEx = Nothing
	
	' Get a Scripting.FileSystem object.
	Dim fso
	Set fso = GetFileSystemObject()
    If Err.Number <> 0 Then SaveCurrentErrorContext "BindTextStreamToFileEx() - Error getting a Scripting.FileSystem object" : Exit Function

    ' Open the specified file in the specified mode.
    Dim stm
    Set stm = fso.OpenTextFile(strFileName, ForReading, False, tsAsciiUnicodeMode)
    If Err.Number <> 0 Then SaveCurrentErrorContext "BindTextStreamToFileEx() - Error opening file. strFileName = """ & strFileName & """" : Exit Function

	Set BindTextStreamToFileEx = stm
End Function



'----------------------------------------
'
'   IsUnicodeFile
'
'   Parameters:
'
'       strFileName
'           The name of the file to be checked.
'
'   Description:
'
'       Determines whether or not the specified file is a Unicode text file.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Function IsUnicodeFile(strFileName)
	On Error Resume Next

    ' Default to FALSE.
    IsUnicodeFile = False

	' Get a Scripting.FileSystem object.
	Dim fso
	Set fso = GetFileSystemObject()
    If Err.Number <> 0 Then SaveCurrentErrorContext "IsUnicodeFile() - Error getting a Scripting.FileSystem object" : Exit Function

    ' Open the specified file as an ASCII file.
    Dim stm
    Set stm = fso.OpenTextFile(strFileName, ForReading, False, tsASCII)
    If Err.Number <> 0 Then SaveCurrentErrorContext "IsUnicodeFile() - Error opening file. strFileName = """ & strFileName & """" : Exit Function

    ' See if there is even anything in the file.
    If stm.AtEndOfStream Then
		Exit Function
    End If

    ' Read the stream contents into a string.
    Dim strTemp
    strTemp = stm.Read(2)
    If Err.Number <> 0 Then
        If Err.Number = errInputPastEndOfFile Then
            ClearCurrentErrorContext()
            Exit Function
        Else
            SaveCurrentErrorContext "IsUnicodeFile() - Error reading text string from file. strFileName = """ & strFileName & """"
            Exit Function
        End If
    End If

    ' Check the first two bytes of the string and see if it was a Unicode file.
    If strTemp = Chr(&hFF) & Chr(&hFE) Then
        ' This was a Unicode file so the string won't have gotten loaded the right way. We need to reread it as a Unicode stream.
        IsUnicodeFile = True
    End If
End Function



'----------------------------------------
'
'   LoadStringFromFile
'
'   Parameters:
'
'       strFileName
'           The name of the file to be checked.
'
'   Description:
'
'       Loads the specified file into a string. This function can property handle both
'		ASCII and Unicode files.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Function LoadStringFromFile(strFileName)
	On Error Resume Next

    Dim tsAsciiUnicodeMode
    If IsUnicodeFile(strFileName) Then
        tsAsciiUnicodeMode = tsUnicode
    Else
        tsAsciiUnicodeMode = tsAscii
    End If

    LoadStringFromFile = LoadStringFromFileEx(strFileName, tsAsciiUnicodeMode)
End Function



'----------------------------------------
'
'   LoadStringFromFileEx
'
'   Parameters:
'
'       strFileName
'           The name of the file to be checked.
'
'		tsAsciiUnicodeMode
'			Specifies the mode to use when opening the file.
'
'   Description:
'
'       Loads the specified file into a string.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Function LoadStringFromFileEx(strFileName, tsAsciiUnicodeMode)
	On Error Resume Next

    ' Bind a TextStream object to the file.
    Dim stm
    Set stm = BindTextStreamToFileEx(strFileName, tsAsciiUnicodeMode)
    If Err.Number <> 0 Then SaveCurrentErrorContext "LoadStringFromFileEx() - Error binding TextStream object to file: " & strFileName : Exit Function

    ' See if there is even anything in the file.
    If stm.AtEndOfStream Then
        Exit Function
    End If

    ' Read the stream contents into a string.
    LoadStringFromFileEx = stm.ReadAll
    If Err.Number <> 0 Then SaveCurrentErrorContext "LoadStringFromFileEx() - Error reading text string from TextStream object bound to file: " & strFileName : Exit Function
End Function



'----------------------------------------
'
'   WriteStringToFile
'
'   Parameters:
'
'       strFileName
'           The name of the file to be written to.
'
'       strOutput
'           The string to be written to the file.
'
'   Description:
'
'       Creates/overwrites the specified file and writes the specified string to it.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Sub WriteStringToFile(strFileName, strOutput)
	On Error Resume Next

	' Get a Scripting.FileSystem object.
	Dim fso
	Set fso = GetFileSystemObject()
    If Err.Number <> 0 Then SaveCurrentErrorContext "WriteStringToFile() - Error getting a Scripting.FileSystem object" : Exit Sub

    ' Try and open/overwrite the specified file.
	Dim fil
    Set fil = fso.OpenTextFile(strFileName, ForWriting, True, tsUnicode)
    If Err.Number <> 0 Then SaveCurrentErrorContext "WriteStringToFile() - Error creating file. strFileName = """ & strFileName & """" : Exit Sub

    ' Write the string to the file.
    fil.WriteLine strOutput
    If Err.Number <> 0 Then SaveCurrentErrorContext "WriteStringToFile() - Error writing text string to file." : Exit Sub
End Sub



'----------------------------------------
'
'   DeleteFile
'
'   Parameters:
'
'       strFileName
'           The name of the file to be deleted.
'
'   Description:
'
'       Deletes the specified file.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Sub DeleteFile(strFileName)
	On Error Resume Next

	' Get a Scripting.FileSystem object.
	Dim fso
	Set fso = GetFileSystemObject()
    If Err.Number <> 0 Then SaveCurrentErrorContext "DeleteFile() - Error getting a Scripting.FileSystem object" : Exit Sub

    ' Delete the file
    fso.DeleteFile strFileName
    If Err.Number <> 0 Then SaveCurrentErrorContext "DeleteFile() - Error deleting file: " & strFileName : Exit Sub

End Sub

'======================================================================
'
' CHttpQuery object - hq
'
'======================================================================

' Constants for HttpStatus property.
Const HTTPSTATUS_OK = 200
Const HTTPSTATUS_CREATED = 201
Const HTTPSTATUS_ACCEPTED = 202
Const HTTPSTATUS_MULTISTATUS = 207
Const HTTPSTATUS_BADREQUEST = 400
Const HTTPSTATUS_UNAUTHORIZED = 401
Const HTTPSTATUS_FORBIDDEN = 403
Const HTTPSTATUS_NOTFOUND = 404
Const HTTPSTATUS_INTERNALSERVERERROR = 500

' TODO: When switch over to the new object, this should get removed.
Dim g_fUseNewXmlHttpObject
g_fUseNewXmlHttpObject = Empty

Class CHttpQuery

Private m_xh                ' Will hold the Microsoft.XMLHTTP object that is used to perform the query.
Private m_strHttpCommand	' Will be used to remember what HTTP command was specified in the request. This parameter is case sensitive.
Private m_strRequestUrl		' Will be used to remember the URL that was specified in the request.
Private m_iRequestRangeStart       ' Used in the request header of a DAV Query only.
Private m_iRequestRangeEnd         '   to specify the range of result rows
Private m_cTotalHits        ' Total number of rows returned from the query. 0 means no hit
Private m_fHasMore          ' Flag indicating if there are any more rows for this query
Private m_fIsAsync			' Flag indicating the query is performed Asynchronously


Private Sub Class_Initialize()
									On Error Resume Next
        Set m_xh = Nothing
        m_fIsAsync = False
End Sub
    


'--------------------------------------------------
'
'   HttpCommand
'
'   Description:
'
'       This property contains the HTTP command that was specified in the request.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get HttpCommand()
								On Error Resume Next
    HttpCommand = m_strHttpCommand
End Property



'--------------------------------------------------
'
'   RequestUrl
'
'   Description:
'
'       This property contains the HTTP command that was specified in the request.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get RequestUrl()
								On Error Resume Next
    RequestUrl = m_strRequestUrl
End Property



'--------------------------------------------------
'
'	 IsAsync
'
'	Description:
'
'		Get/Set the value of Async Mode of the query.
'
Public Property Let IsAsync(fAsync)
								On Error Resume Next
	If VarType(fAsync) = vbBoolean Then
	    m_fIsAsync = fAsync
	End If
End Property

Public Property Get IsAsync()
								On Error Resume Next
	IsAsync = m_fIsAsync
End Property



'--------------------------------------------------
'
'   RequestRangeStart
'
'   Description:
'
'       This property specifies the starting offset of the request range if one was
'		specified when the query was started; otherwise, Empty is returned.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get RequestRangeStart()
								On Error Resume Next
    RequestRangeStart = m_iRequestRangeStart
End Property



'--------------------------------------------------
'
'   RequestRangeEnd
'
'   Description:
'
'       This property specifies the ending offset of the request range if one was
'		specified when the query was started; otherwise, Empty is returned.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get RequestRangeEnd()
								On Error Resume Next
    RequestRangeEnd = m_iRequestRangeEnd
End Property



'--------------------------------------------------
'
'   IsQueryFinished
'
'   Description:
'
'       This property determines whether or not the query that is encapsulated within
'       the specified query object has completed.  If a query has not been started in
'       this object, then this property will return False.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get IsQueryFinished()
								On Error Resume Next

    

    ' Default to False.
    IsQueryFinished = False

    ' If a query hasn't been started, then we can just return the default False value.
    If m_xh Is Nothing Then Exit Property


    ' See if the object is done.
    If m_xh.ReadyState = 4 Then
        IsQueryFinished = True
    Else
    End If
End Property




'--------------------------------------------------
'
'   HasQueryBeenStarted
'
'   Description:
'
'       This property determines whether or not a query has be started using this object.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get HasQueryBeenStarted()
								On Error Resume Next

    If m_xh Is Nothing Then
        HasQueryBeenStarted = False
    Else
        HasQueryBeenStarted = True
    End If
End Property




'--------------------------------------------------
'
'   QueryHttpStatus
'
'   Description:
'
'       This property returns the HTTP status code of the query.
'
'   Return Value:
'       If this function performed correctly, the return value is an integer containing the
'       HTTP result status. If the query has not finished yet or any errors occurred, then Empty
'       will be returned.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get QueryHttpStatus()
								On Error Resume Next

    

    ' If a query hasn't been started, then we don't need to do anything.
    If m_xh Is Nothing Then Exit Property

    ' Confirm that the query results are finished.
    If Not IsQueryFinished() Then Exit Property
    If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_QueryHttpStatus() - Error determining whether or not query is finished yet." : Exit Property

    ' Return the response string back to the caller.
    QueryHttpStatus = m_xh.Status
End Property



'--------------------------------------------------
'
'   QueryResults
'
'   Description:
'
'       This property returns the results of the query as a string.
'
'   Return Value:
'       If this function performed correctly, the return value is a string containing the
'       query results. If the query has not finished yet or any errors occurred, then Empty
'       will be returned.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get QueryResults()
								On Error Resume Next

    

    ' If a query hasn't been started, then we don't need to do anything.
    If m_xh Is Nothing Then Exit Property

    ' Confirm that the query results are finished.
    If Not IsQueryFinished() Then Exit Property
    If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_QueryResults() - Error determining whether or not query is finished yet." : Exit Property

    ' Return the response string back to the caller.
    QueryResults = m_xh.ResponseText

End Property



'--------------------------------------------------
'
'   QueryResponseHeaders
'
'   Description:
'
'       This property returns all of the response headers as a string. The headers are
'		delimited with CRLFs.
'
'   Return Value:
'
'       If this function performed correctly, the return value is a string containing the
'       response headers. If the query has not finished yet or any errors occurred, then
'       Empty will be returned.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get QueryResponseHeaders()
								On Error Resume Next

    

    ' If a query hasn't been started, then we don't need to do anything.
    If m_xh Is Nothing Then Exit Property

    ' Confirm that the query results are finished.
    If Not IsQueryFinished() Then Exit Property
    If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_QueryResponseHeaders() - Error determining whether or not query is finished yet." : Exit Property

    ' Return the response headers string back to the caller.
    QueryResponseHeaders = m_xh.GetAllResponseHeaders

End Property



'--------------------------------------------------
'
'   QueryResponseHeader
'
'	Parameters:
'
'		strHeaderName
'			Specifies the name of the header value to be returned.
'
'   Description:
'
'       This property returns the value of the specified response header.
'
'   Return Value:
'
'       If this function performed correctly, the return value is a string containing the
'       response header value. If the query has not finished yet or any errors occurred,
'       then Empty will be returned.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get QueryResponseHeader(strHeaderName)
								On Error Resume Next

    

    ' If a query hasn't been started, then we don't need to do anything.
    If m_xh Is Nothing Then Exit Property

    ' Confirm that the query results are finished.
    If Not IsQueryFinished() Then Exit Property
    If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_QueryResponseHeader() - Error determining whether or not query is finished yet." : Exit Property

    ' Return the response headers string back to the caller.
    QueryResponseHeader = m_xh.GetResponseHeader(strHeaderName)

End Property




'--------------------------------------------------
'
'   AreQueryXmlResultsValid
'
'   Description:
'
'       This property specifies whether or not there was a parsing error when parsing
'		the XML results.
'
'   Return Value:
'       If this function performed correctly, the return value specifies whether or not
'		there was a parsing error when parsing the XML results. If the query has not
'		finished yet or any errors occurred, then Empty will be returned.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get AreQueryXmlResultsValid()
								On Error Resume Next

    

    ' Prep for error exit.
    AreQueryXmlResultsValid = False

    ' If a query hasn't been started, then we don't need to do anything.
    If m_xh Is Nothing Then Exit Property

    ' Confirm that the query results are finished.
    If Not IsQueryFinished() Then Exit Property
    If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_AreQueryXmlResultsValid() - Error determining whether or not query is finished yet." : Exit Property

	' Confirm that the HTTP request succeeded.
	If (QueryHttpStatus() <> HTTPSTATUS_OK) And (QueryHttpStatus() <> HTTPSTATUS_MULTISTATUS) Then Exit Property
	
    ' See if there was a parsing error when the XML response was loaded into the XMLDOM object.
	' TODO: Get rid of the check for the new object.
    If m_xh.ResponseXML.ParseError.ErrorCode = 0 Then
		AreQueryXmlResultsValid = True
    End If
End Property



'--------------------------------------------------
'
'   QueryXmlResults
'
'   Description:
'
'       This property returns the results of the query as an XMLDOM object.
'
'   Return Value:
'       If this function performed correctly, the return value is a pointer to an XMLDOM
'       object containing the parsed query results. If the query has not finished yet or
'       any errors occurred, then Nothing will be returned.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get QueryXmlResults()
								On Error Resume Next

    

    ' Prep for error exit.
    Set QueryXmlResults = Nothing

    ' If a query hasn't been started, then we don't need to do anything.
    If m_xh Is Nothing Then Exit Property

    ' Confirm that the query results are finished and that we have a valid XML response.
    If Not IsQueryFinished() Then Exit Property
    If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_QueryXmlResults() - Error determining whether or not query is finished yet." : Exit Property
	If Not AreQueryXmlResultsValid() Then Exit Property
    If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_QueryXmlResults() - Error determining whether or not XML results are valid." : Exit Property

    ' Return the XMLDOM object back to the caller.
    Set QueryXmlResults = m_xh.ResponseXML
End Property



'--------------------------------------------------
'
'   TotalHits
'
'   Description:
'
'       This property returns the total hits from the response header.
'
'   Return Value:
'       If this function performed correctly, the return value is a the number
'       of hits for the current query. If the query has not finished yet or
'       any errors occurred, then -1 will be returned.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get TotalHits()
								On Error Resume Next

    ' Prep for error exit.
    TotalHits = -1

    If IsEmpty(m_cTotalHits) Then
		' If a query hasn't been started, then we don't need to do anything.
		If m_xh Is Nothing Then Exit Property

		' Confirm that the query results are finished and that we have a valid XML response.
		If Not IsQueryFinished() Then Exit Property
		If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_TotalHits() - Error determining whether or not query is finished yet." : Exit Property
		If Not AreQueryXmlResultsValid() Then Exit Property
		If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_TotalHits() - Error determining whether or not XML results are valid." : Exit Property

        ' Try to get the Content-Range response header and parse out the total number of rows. The format is like: rows=0-5; total=6
        Dim strHitRange, iTagTotal
        strHitRange = m_xh.GetResponseHeader("Content-Range")
        If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_TotalHits() - Error getting the response header." : Exit Property
        iTagTotal = InStr(strHitRange, "total=")
		If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_TotalHits() - Error finding the total within the content-range header text string." : Exit Property
		If iTagTotal > 0 Then
	        m_cTotalHits = CLng(Mid(strHitRange, iTagTotal + Len("total=")))
		End If
    End If

    ' return the value in member variable
    TotalHits = m_cTotalHits    
End Property



'--------------------------------------------------
'
'   HasMore
'
'   Description:
'
'       This property returns the flag indicating if there are more rows for this query.
'       Empty is returned for INVOKE query.
'
'   Return Value:
'       If this function performed correctly, the return value is a boolean value
'           True = Yes, there is more
'           False = No, no more results.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Property Get HasMore()
								On Error Resume Next

    ' Prep for error exit.
    HasMore = Empty

    If IsEmpty(m_fHasMore) Then 
        
    
        ' For invoke queries, it always return false
        If m_strHttpCommand = "INVOKE" Then  HasMore = False : Exit Property

        ' If a query hasn't been started, then we don't need to do anything.
        If m_xh Is Nothing Then Exit Property

        ' Confirm that the query results are finished.
        If Not IsQueryFinished() Then Exit Property
        If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_HasMore() - Error determining whether or not query is finished yet." : Exit Property

    
        ' Try to get the string value from the response header
        Dim strMoreRows
        strMoreRows = m_xh.GetResponseHeader("MS-Search-MoreRows")
        If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::get_HasMore() - Error getting the response header." : Exit Property

        ' set the member variable so that next time it can be returned immediately
        m_fHasMore = ( "t" = strMoreRows )
        ' Return true if strMoreRows = "t"
    End If
    
    HasMore = m_fHasMore
End Property



'--------------------------------------------------
'
'   StartQuery
'
'   Parameters:
'
'       strCommand
'           Specifies the HTTP command being invoked.
'
'       strUrl
'           Specifies the URL.
'
'       strRequest
'           Specifies the request to be sent.
'
'       strContentType
'           Specifies the content-type HTTP header value to be used. If this string
'           is empty or this parameter is Null or Empty, the default value used will
'           be "text/xml".
'
'		nDepth
'			Specifies the depth HTTP header value to be used. If this parameter is either
'			Empty or when coerced to a string an empty string is produced, then the header
'			value will not be added to the request.
'
'       iStart
'           Specified a zero-based index for the query to get rows beginning with that as
'			row number. Only when iEnd is a positive number (>0) AND not INVOKE query,
'			because INVOKE query does not support range.
'
'       iEnd
'           Specified a zero-based index for the query to get rows ending with that row
'			number. If the value is not a number or 0 or negative number, the query
'           will ignore the range (i.e. a full query will be launched)
'
'		cMaxRows
'			Specifies the maximum number of rows to search for. Since this is a count, it
'			is one-based not zero-based like iStart and iEnd. This number should be
'			greater than or equal to iEnd or you will not get the full requested range. If
'			this parameter is less than zero, it will be ignored.
'
'		dictRequestHeaders
'			Specifies a Scripting.Dictionary object that contains request header name/value
'			pairs. If you do not need any custom headers in your request, then this parameter
'            can be specified as Nothing.
'
'   Description:
'
'       This function will start the specified search query. If a query has already
'       been started using this object, then this function will fail.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Sub StartQuery(strCommand, strUrl, strRequest, strContentType, nDepth, iStart, iEnd)
								On Error Resume Next
	StartQueryEx strCommand, strUrl, strRequest, strContentType, nDepth, iStart, iEnd, -1, Nothing
End Sub

Public Sub StartQueryEx(strCommand, strUrl, strRequest, strContentType, nDepth, iStart, iEnd, cMaxRows, dictRequestHeaders)
								On Error Resume Next

	' If needed, determine whether or not the new XML object is installed. If the global
	' variable g_fUseNewXmlHttpObject is either False or True, then its value is respected
	' without question. If it is Empty, that will cause this block of code to check for
	' the existence of the new object on this particular machine and use it if it's installed.
	' TODO: When switch over to the new object, this should get removed.
	If IsEmpty(g_fUseNewXmlHttpObject) Then
		g_fUseNewXmlHttpObject = False ' Set default value.
		Dim xhTemp
		Set xhTemp = CreateObject("Msxml2.ServerXMLHTTP.3.0")
		If (Err.Number <> 0) Then
			ClearCurrentErrorContext()
		Else
			If IsObject(xhTemp) Then
				If Not (xhTemp Is Nothing) Then
					g_fUseNewXmlHttpObject = True
				End If
			End If
		End If
	End If

    ' If a query has already been started, then we need to fail.
    If Not (m_xh Is Nothing) Then
        Err.Raise 1, "StartQueryEx", "Cannot start more than one query with this object."
        Exit Sub
    End If

	' Instantiate an XMLHTTP object that we will use to perform the query.
	If g_fUseNewXmlHttpObject Then
		Set m_xh = CreateObject("Msxml2.ServerXMLHTTP.3.0")
		If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartQueryEx() - Error instantiating ServerXMLHTTP object." : Exit Sub
	Else
		Set m_xh = CreateObject("Microsoft.XMLHTTP")
		If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartQueryEx() - Error instantiating XMLHTTP object." : Exit Sub
	End If

    ' Start the query. The m_fIsAsync controls the mode.
    m_xh.Open strCommand, strUrl, m_fIsAsync, GetAuthUser(True), GetAuthPassword(True)
    If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartQueryEx() - Error opening """ & strCommand & """ request on XMLHTTP object to URL """ & strUrl & """" : Exit Sub
	If Len(CStr(strContentType)) > 0 Then
		m_xh.SetRequestHeader "content-type", strContentType
		If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartQueryEx() - Error setting request header 'content-type' on XMLHTTP object to """ & strContentType & """" : Exit Sub
	End If
	If Not IsEmpty(nDepth) Then
		Dim strDepth
		strDepth = CStr(nDepth)
		If Len(strDepth) > 0 Then
			m_xh.SetRequestHeader "Depth", strDepth
			If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartQueryEx() - Error setting request header 'depth' on XMLHTTP object to """ & strDepth & """" : Exit Sub
		End If
	End If

	' Remember the command and the URL that were used.
	m_strHttpCommand = strCommand
	m_strRequestUrl = strUrl

    ' Only use the range in non-invoke queries
    If (m_strHttpCommand <> "INVOKE") And (iEnd > 0) Then
        ' The range will be specfied in the request head.
        m_iRequestRangeStart = iStart
        m_iRequestRangeEnd   = iEnd

        m_xh.SetRequestHeader "Range", "rows=" & CStr(iStart) & "-" & CStr(iEnd)
        If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartQueryEx() - Error setting request header 'Range' on XMLHTTP object to ""rows=" & CStr(iStart) & "-" & CStr(iEnd) & """" : Exit Sub
        m_xh.SetRequestHeader "MS-Search-TotalHits", "t"
        If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartQueryEx() - Error setting request header 'MS-Search-TotalHits' on XMLHTTP object to ""t""" : Exit Sub
		m_xh.SetRequestHeader "MS-Search-UseContentIndex", "t"
		If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartQueryEx() - Error setting request header 'MS-Search-UseContentIndex' on XMLHTTP object to ""t""" : Exit Sub
		If (Not IsEmpty(cMaxRows)) And (cMaxRows > 0) Then
	        m_xh.SetRequestHeader "MS-SEARCH-MAXROWS", CStr(cMaxRows)
	        If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartQueryEx() - Error setting request header 'MS-SEARCH-MAXROWS' on XMLHTTP object to ""CStr(iEnd)""" : Exit Sub
		End If
    End If

    



	' Set any additional request headers that were specified.
	Dim strKeyName
	If IsObject(dictRequestHeaders) Then
		If Not (dictRequestHeaders Is Nothing) Then
			For Each strKeyName In dictRequestHeaders
				If Not IsEmpty(strKeyName) Then
					m_xh.SetRequestHeader strKeyName, dictRequestHeaders(strKeyName)
			        If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartQueryEx() - Error setting request header on XMLHTTP object." : Exit Sub
				End If
			Next
		End If
	End If


    m_xh.Send strRequest
    If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartQueryEx() - Error sending request via XMLHTTP object." : Exit Sub
End Sub



'--------------------------------------------------
'
'   StartSearchQuery
'
'   Parameters:
'
'       strSqlQuery
'           Specifies a string containing the SQL query to be performed. This string
'           should include only the SQL query. It should not contain any of the DAV
'           search request XML.
'
'       iStart
'           Specified a zero-based index for the query to get rows beginning with that as
'			row number. Only when iEnd is a positive number (>0) AND not INVOKE query,
'			because INVOKE query does not support range.
'
'       iEnd
'           Specified a zero-based index for the query to get rows ending with that row
'			number. If the value is not a number or 0 or negative number, the query
'           will ignore the range (i.e. a full query will be launched)
'
'		cMaxRows
'			Specifies the maximum number of rows to search for. Since this is a count, it
'			is one-based not zero-based like iStart and iEnd. This number should be
'			greater than or equal to iEnd or you will not get the full requested range. If
'			this parameter is less than zero, it will be ignored.
'
'   Description:
'
'       This function will start the specified search query. The DAV searchrequest XML
'       wrapper will automatically be added around the specified SQL query string. If a
'       query has already been started using this object, then this function will fail.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Sub StartSearchQuery(strSqlQuery, iStart, iEnd, cMaxRows)
								On Error Resume Next

    Dim strRequest
    strRequest = _
            "<?xml version=""1.0"" encoding=""utf-8""?>" & vbCRLF & _
            "<a:searchrequest xmlns:a=""DAV:"">" & vbCRLF & _
            "  <a:sql>" & vbCRLF & _
			Replace(Replace(Replace(strSqlQuery, "&", "&amp;"), "<", "&lt;"), ">", "&gt;") & vbCRLF & _
            "  </a:sql>" & vbCRLF & _
            "</a:searchrequest>"
    
    StartQueryEx "SEARCH", GetWorkspaceUrl(), strRequest, "text/xml", Empty, iStart, iEnd, cMaxRows, Nothing
    If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartSearchQuery() - Error starting query." : Exit Sub
End Sub



'--------------------------------------------------
'
'   StartInvokeQuery
'
'   Parameters:
'
'       strUrl
'           The URL to perform Invoke query on.
'
'			NOTE: This URL must already be in canonicalized UTF-8 format.
'
'       strInvokeQuery
'           Specifies a string containing the Invoke query to be performed. This string
'           should include a complete Invoke query string.
'
'   Description:
'
'       This function will start the specified Invoke query. It turns around and call
'       the StartQuery with INVOKE parameters and specifies no range. 
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Sub StartInvokeQuery(strUrl, strInvokeQuery)
								On Error Resume Next

    StartQuery "INVOKE", strUrl, strInvokeQuery, "text/xml", Empty, Empty, Empty
    If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::StartInvokeQuery() - Error starting invokequery." : Exit Sub
End Sub



'--------------------------------------------------
'
'   WaitForQueryCompletion
'
'   Parameters:
'
'   Description:
'
'       This function will block until all of the results of this query have been
'       returned. If a query has not been started in this object, then this function
'       will simply return immediately without raising an error.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'

Public Sub WaitForQueryCompletion()
								On Error Resume Next

    

    ' If a query hasn't been started, then we don't need to do anything.
    If m_xh Is Nothing Then Exit Sub

    ' Block until the query results are all returned.
	If m_fIsAsync Then
		Dim fIsGoodRequest
		fIsGoodRequest = m_xh.WaitForResponse()	
		' without a timeout value, it will wait until done.
		
		' the .waitForResponse( ) return "False" when the query is invalid
		If Not fIsGoodRequest Then 
		    Err.Raise 1, "WaitForQueryCompletion", "Invalid query."
		    Exit Sub
		End If
		
	    If Err.Number <> 0 Then SaveCurrentErrorContext "CHttpQuery::WaitForQueryCompletion() - Error waiting for response." : Exit Sub
	End If

End Sub

End Class 'CHttpQuery



'======================================================================
'
' Security helper functions
'
'======================================================================


'----------------------------------------
'
'   GetAuthUser
'
'	Parameters:
'
'		fReturnNullIfEmpty
'			Specifies that Null should be returned instead of Empty.
'
'   Description:
' 
'		This function will check the user authentication parameters
'		in the Request.ServerVariables() collection. If the authentication
'		type was Basic, then the value of the AUTH_USER variable will be
'		returned. If the authentication type was anything else, then Empty
'		or Null will be returned depending on the parameter fReturnNullIfEmpty.
'
'   Return Value:
'
'       The return value can be one of three things: a string containing the
'		user name, Empty, or Null.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'
Dim g_strAuthUser
Function GetAuthUser(fReturnNullIfEmpty)
	On Error Resume Next

	If Not IsEmpty(g_strAuthUser) Then
		GetAuthUser = g_strAuthUser

	Else
		If fReturnNullIfEmpty Then
			GetAuthUser = Null
		Else
			GetAuthUser = Empty
		End If
	End If
End Function



'----------------------------------------
'
'   GetAuthPassword
'
'	Parameters:
'
'		fReturnNullIfEmpty
'			Specifies that Null should be returned instead of Empty.
'
'   Description:
' 
'		This function will check the user authentication parameters
'		in the Request.ServerVariables() collection. If the authentication
'		type was Basic, then the value of the AUTH_PASSWORD variable will be
'		returned. If the authentication type was anything else, then Empty
'		or Null will be returned depending on the parameter fReturnNullIfEmpty.
'
'   Return Value:
'
'       The return value can be one of three things: a string containing the
'		user password, Empty, or Null.
'
'   Error Handling:
'
'       If this function performed correctly, Err.Number will be zero. If any errors
'       occured, Err.Number will be non-zero and the operation that this subroutine
'       was attempting to perform when the error occured will be placed in the global
'       variable g_strErrorContext.
'
Dim g_strAuthPassword
Function GetAuthPassword(fReturnNullIfEmpty)
	On Error Resume Next

	If Not IsEmpty(g_strAuthPassword) Then
		GetAuthPassword = g_strAuthPassword

	Else
		If fReturnNullIfEmpty Then
			GetAuthPassword = Null
		Else
			GetAuthPassword = Empty
		End If
	End If
End Function




'======================================================================
'
' Main-line code starts here
'
'======================================================================

Const strStandardOutputFileTemplate = "%TEMP%\Out.xml"

' We need a WScript.Shell object for expanding environment strings.
Dim g_wshShell
Set g_wshShell = CreateObject("WScript.Shell")
If Err.Number <> 0 Then DisplayErrorSummaryAndExit "Error instantiating WScript.Shell object."


' Check for the help switch.
If wscript.Arguments.Count >= 1 Then
    If (CStr(wscript.Arguments(0))= "/?") Or (CStr(wscript.Arguments(0))= "-?") Then
        ShowUsageAndExit ""
    End If
End If

' Validate that at least the required parameters were specified.
If wscript.Arguments.Count < 2 Then ShowUsageAndExit "Error: Not enough parameters specified."

' Disable the code we import from CHttpQuery.vbs that will automatically test for the
' existence of the newer ServerXMLHTTP object and use it.
g_fUseNewXmlHttpObject = False

' Walk through the parameters and switches and load them into variables.
Dim iParam, iActualParameter, strParam
Dim fQuiet, strHttpVerbParam, strDestUrlParam, strRequestFileName, strResponseOutputFileName, strSearchKeyword
Dim fListResponseToConsole, strNameAndValue, ichEQ, strName, strValue
Dim fStripEncodingUtf8Header, fViewInIE, cIterations, fDumpResponseHeaders, dictRequestHeaders
Dim iRequestRangeStart, iRequestRangeEnd, iRequestMaxRows
iActualParameter = 0
fQuiet = False
fListResponseToConsole = False
fStripEncodingUtf8Header = True
fViewInIE = False
cIterations = 1
fDumpResponseHeaders = False
Set dictRequestHeaders = CreateObject("Scripting.Dictionary")
If Err.Number <> 0 Then DisplayErrorSummaryAndExit "Error instantiating Dictionary object."

For iParam = 0 To wscript.Arguments.Count - 1
	strParam = CStr(wscript.Arguments(iParam))
	If Len(strParam) > 0 Then
		If Left(strParam, 1) = "/" Then
			' We have come across a switch.
			If LCase(strParam) = "/new" Then
				' Setting this to Empty will cause the code in CHttpQuery to check for existence of the new ServerXMLHTTP
				' object and use it only if it's present.
				g_fUseNewXmlHttpObject = Empty
			ElseIf LCase(strParam) = "/q" Then
				fQuiet = True
			ElseIf LCase(Left(strParam, 4)) = "/out" Then
				strResponseOutputFileName = Mid(strParam, 6)
				If Len(strResponseOutputFileName) <= 0 Then
					strResponseOutputFileName = g_wshShell.ExpandEnvironmentStrings(strStandardOutputFileTemplate)
				End If
			ElseIf LCase(Left(strParam, 2)) = "/c" Then
				cIterations = CLng(Mid(strParam, 4))
				If cIterations <= 0 Then
					WriteOutput "Ignoring negative count specified in /c option."
					cIterations = 1
				End If
			ElseIf LCase(Left(strParam, 2)) = "/k" Then
				strSearchKeyword = Mid(strParam, 4)
				If Len(strSearchKeyword) <= 0 Then
					strSearchKeyword = "foo"
				End If
			ElseIf LCase(Left(strParam, 2)) = "/u" Then
				g_strAuthUser = Mid(strParam, 4)
				If Len(g_strAuthUser) > 0 Then
					WriteOutput "Override user name specified: " & g_strAuthUser
				End If
			ElseIf LCase(Left(strParam, 2)) = "/p" Then
				g_strAuthPassword = Mid(strParam, 4)
				If Len(g_strAuthPassword) > 0 Then
					WriteOutput "Override password specified (value not shown). "
				End If
			ElseIf LCase(Left(strParam, 2)) = "/h" Then
				strNameAndValue = Mid(strParam, 4)
				If Len(strNameAndValue) <= 0 Then
					WriteOutput "Ignoring invalid /h switch. No name/value specified."
				End If
				ichEQ = InStr(strNameAndValue, "=")
				If ichEQ <= 0 Then
					' The header gets set without a value.
					strName = strNameAndValue
					strValue = Empty
				Else
					strName = Left(strNameAndValue, ichEQ - 1)
					strValue = Mid(strNameAndValue, ichEQ + 1)
				End If
				dictRequestHeaders(strName) = strValue
				If Err.Number <> 0 Then DisplayErrorSummaryAndExit "Error adding name/value """ & strNameAndValue & """ to Dictionary object."
			ElseIf LCase(Left(strParam, 2)) = "/b" Then
				iRequestRangeStart = CLng(Mid(strParam, 4))
				If Err.Number <> 0 Then
					Err.Clear
					iRequestRangeStart = Empty
					WriteOutput "Ignoring invalid /b parameter. Error converting parameter value to an integer."
				End If
			ElseIf LCase(Left(strParam, 2)) = "/e" Then
				iRequestRangeEnd = CLng(Mid(strParam, 4))
				If Err.Number <> 0 Then
					Err.Clear
					iRequestRangeEnd = Empty
					WriteOutput "Ignoring invalid /e parameter. Error converting parameter value to an integer."
				End If
			ElseIf LCase(Left(strParam, 2)) = "/x" Then
				iRequestMaxRows = CLng(Mid(strParam, 4))
				If Err.Number <> 0 Then
					Err.Clear
					iRequestMaxRows = Empty
					WriteOutput "Ignoring invalid /x parameter. Error converting parameter value to an integer."
				End If
			ElseIf LCase(strParam) = "/list" Then
				fListResponseToConsole = True
			ElseIf LCase(strParam) = "/nostrip" Then
				fStripEncodingUtf8Header = False
			ElseIf LCase(strParam) = "/ie" Then
				fViewInIE = True
			ElseIf LCase(strParam) = "/d" Then
				fDumpResponseHeaders = True
			ElseIf strParam = "/?" Then
			    ShowUsageAndExit ""
			Else
				WriteOutput "Ignoring unknown switch """ & strParam & """."
			End If
		Else
			' We have encountered a real parameter.
			Select Case iActualParameter
				Case 0
					strHttpVerbParam = strParam
					iActualParameter = iActualParameter + 1
				Case 1
					strDestUrlParam = strParam
					iActualParameter = iActualParameter + 1
				Case 2
					strRequestFileName = strParam
					iActualParameter = iActualParameter + 1
				Case Else
					WriteOutput "Ignoring extra parameter """ & strParam & """."
			End Select
		End If
	End If
Next

' Check for the required parameters.
If Len(strHttpVerbParam) = 0 Then ShowUsageAndExit "Error: The HttpVerb parameter is missing or empty."
If Len(strDestUrlParam) = 0 Then ShowUsageAndExit "Error: The Url parameter is missing or empty."

' Setup the "hidden" query parameters using default values that are keyed off the verb type.
Dim strRequest, strContentType, nDepth
If LCase(strHttpVerbParam) = "get" Then
	strRequest = Empty
	strContentType = Empty
	nDepth = Empty
ElseIf LCase(strHttpVerbParam) = "delete" Then
	strRequest = Empty
	strContentType = Empty
	nDepth = Empty
ElseIf LCase(strHttpVerbParam) = "invoke" Then
	strRequest = _
			"<request>" & vbCRLF & _
			"	<selector>enumfolder</selector>" & vbCRLF & _
			"	<parameters>" & vbCRLF & _
			"		<param dt=""str"">(NOT (""DAV:ishidden"" = TRUE)) AND ((CAST(""urn:schemas-microsoft-com:publishing:IsHiddenInPortal"" AS ""boolean"") = FALSE) OR (CAST(""urn:schemas-microsoft-com:publishing:IsHiddenInPortal"" AS ""boolean"") IS NULL))</param>" & vbCRLF & _
			"		<param dt=""int"">1</param>" & vbCRLF & _
			"		<param dt=""array"">" & vbCRLF & _
			"			<param dt=""str"">ORDER BY CAST(""DAV:iscollection"" AS ""boolean"") DESC, ""DAV:displayname"" ASC</param>" & vbCRLF & _
			"		</param>" & vbCRLF & _
			"		<param dt=""int"">28</param>" & vbCRLF & _
			"		<param dt=""array"">" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:office:office#Title</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:office:office#Author</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:office:office#Description</param>" & vbCRLF & _
			"			<param dt=""str"">cast(""urn:schemas-microsoft-com:publishing:Categories"" as ""mv.string"")</param>" & vbCRLF & _
			"			<param dt=""str"">cast(""urn:schemas-microsoft-com:office:office#Keywords"" as ""mv.string"")</param>" & vbCRLF & _
			"			<param dt=""str"">DAV:contentclass</param>" & vbCRLF & _
			"			<param dt=""str"">DAV:ishidden</param>" & vbCRLF & _
			"			<param dt=""str"">DAV:getcontentlength</param>" & vbCRLF & _
			"			<param dt=""str"">DAV:getlastmodified</param>" & vbCRLF & _
			"			<param dt=""str"">DAV:creationdate</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas.microsoft.com:fulltextqueryinfo:description</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas.microsoft.com:fulltextqueryinfo:noindex</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:BaseDoc</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:checkedoutby</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:documentstate</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:FriendlyVersionID</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:IsAVersion</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:IsAWorkingCopy</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:IsCheckedOut</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:IsHiddenInPortal</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:LastUnapprovedVersion</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:publishedby</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:publishingmodel</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:ShortcutTarget</param>" & vbCRLF & _
			"			<param dt=""str"">urn:schemas-microsoft-com:publishing:WorkingCopy</param>" & vbCRLF & _
			"			<param dt=""str"">cast(""urn:schemas-microsoft-com:publishing:currentapprovers"" as ""mv.string"")</param>" & vbCRLF & _
			"			<param dt=""str"">cast(""urn:schemas-microsoft-com:publishing:savedapprovers"" as ""mv.string"")</param>" & vbCRLF & _
			"			<param dt=""str"">cast(""urn:schemas-microsoft-com:publishing:savedapproversleft"" as ""mv.string"")</param>" & vbCRLF & _
			"		</param>" & vbCRLF & _
			"	</parameters>" & vbCRLF & _
			"</request>" & vbCRLF
	strContentType = "text/xml"
	nDepth = Empty
ElseIf LCase(strHttpVerbParam) = "search" Then
	strRequest = _
			"<?xml version=""1.0""?>" & vbCRLF & _
			"<a:searchrequest xmlns:a=""DAV:"">" & vbCRLF & _
			"	<a:sql>" & vbCRLF & _
			"		SELECT RANK," & vbCRLF & _
			"			""DAV:displayname""," & vbCRLF & _
			"			""DAV:href""," & vbCRLF & _
			"			""urn:schemas-microsoft-com:office:office#Keywords""," & vbCRLF & _
			"			""urn:schemas-microsoft-com:office:office#Subject""," & vbCRLF & _
			"			""urn:schemas-microsoft-com:office:office#Title""," & vbCRLF & _
			"			""urn:schemas.microsoft.com:fulltextqueryinfo:description""," & vbCRLF & _
			"			""DAV:contentclass""," & vbCRLF & _
			"			""urn:schemas-microsoft-com:publishing:AutoCategories""," & vbCRLF & _
			"			""urn:schemas-microsoft-com:publishing:BestBetKeywords""," & vbCRLF & _
			"			""urn:schemas-microsoft-com:publishing:Categories""," & vbCRLF & _
			"			""urn:schemas-microsoft-com:office:office#Description""," & vbCRLF & _
			"			""urn:schemas.microsoft.com:fulltextqueryinfo:sourcegroup""," & vbCRLF & _
			"			""urn:schemas.microsoft.com:source:host""," & vbCRLF & _
			"			""urn:schemas-microsoft-com:publishing:isdoclibrarycontent""" & vbCRLF & _
			"		FROM" & vbCRLF & _
			"			SCOPE('DEEP TRAVERSAL OF ""/Tahoe""')" & vbCRLF & _
			"		WHERE" & vbCRLF & _
			"			WITH (" & vbCRLF & _
			"					""DAV:contentclass"":0," & vbCRLF & _
			"					""DAV:href"":0," & vbCRLF & _
			"					""DAV:parentname"":0," & vbCRLF & _
			"					""DAV:searchrequest"":0," & vbCRLF & _
			"					""http://schemas.microsoft.com/exchange/permanenturl"":0," & vbCRLF & _
			"					""urn:schemas-microsoft-com:publishing:BaseDoc"":0," & vbCRLF & _
			"					""urn:schemas-microsoft-com:publishing:LastApprovedVersion"":0," & vbCRLF & _
			"					""urn:schemas-microsoft-com:publishing:LastUnapprovedVersion"":0," & vbCRLF & _
			"					""urn:schemas-microsoft-com:publishing:AutoCategories"":0," & vbCRLF & _
			"					""urn:schemas-microsoft-com:publishing:Categories"":0," & vbCRLF & _
			"					""urn:schemas-microsoft-com:publishing:WorkspaceName"":0," & vbCRLF & _
			"					""urn:schemas-microsoft-com:exch-data:schema-collection-ref"":0," & vbCRLF & _
			"					""urn:schemas-microsoft-com:office:office#Description"":0," & vbCRLF & _
			"					""urn:schemas-microsoft-com:office:office#Keywords"":0.8," & vbCRLF & _
			"					""urn:schemas-microsoft-com:office:office#Subject"":0.8," & vbCRLF & _
			"					""urn:schemas-microsoft-com:office:office#Title"":0.8," & vbCRLF & _
			"					""urn:schemas.microsoft.com:fulltextqueryinfo:description"":0.8," & vbCRLF & _
			"					""urn:schemas.microsoft.com:fulltextqueryinfo:sourcegroup"":0," & vbCRLF & _
			"					""urn:schemas.microsoft.com:source:host"":0," & vbCRLF & _
			"					*:0.5" & vbCRLF & _
			"				) AS #WeightedProps" & vbCRLF & _
			"			(""urn:schemas-microsoft-com:publishing:BestBetKeywords"" = SOME ARRAY ['%KEYWORD%']) " & vbCRLF & _
			"			OR" & vbCRLF & _
			"			CONTAINS(""urn:schemas-microsoft-com:publishing:BestBetKeywords"", '""%KEYWORD%""') RANK BY COERCION(absolute, 999)" & vbCRLF & _
			"			OR" & vbCRLF & _
			"			((FREETEXT(""urn:schemas-microsoft-com:publishing:BestBetKeywords"", '""%KEYWORD%""') RANK BY COERCION(multiply, 0.5)) RANK BY COERCION(add, 500)) " & vbCRLF & _
			"			OR" & vbCRLF & _
			"			(FREETEXT(#WeightedProps, '""%KEYWORD%""') RANK BY COERCION(multiply, 0.6)) " & vbCRLF & _
			"		ORDER BY" & vbCRLF & _
			"			RANK DESC" & vbCRLF & _
			"	</a:sql>" & vbCRLF & _
			"</a:searchrequest>" & vbCRLF & _
			""

	strContentType = "text/xml"
	nDepth = Empty
ElseIf LCase(strHttpVerbParam) = "propfind" Then
	strRequest = _
			"<?xml version=""1.0""?>" & vbCRLF & _
			"<a:propfind xmlns:a=""DAV:"">" & vbCRLF & _
			"  <a:allprop/>" & vbCRLF & _
			"</a:propfind>"
	strContentType = "text/xml"
	nDepth = 1
Else
	ShowUsageAndExit "Error: The value """ & strHttpVerbParam & """ is invalid for the HttpVerb parameter."
End If

If Len(strRequestFileName) > 0 Then
	' A file name was supplied. We need to use the contents of that file for the request.
	strRequest = LoadStringFromFile(strRequestFileName)
	If Err.Number <> 0 Then DisplayErrorSummaryAndExit "Error instantiating CHttpQuery object."
	If Len(strRequest) <= 0 Then
		WriteOutput "WARNING: The specified RequestFile is empty."
	Else
		If Not fQuiet Then
			WriteOutput "Using request text loaded from file " & strRequestFileName
		End If
	End If
End If

' Replace any tokens in the request string.
strRequest = Replace(strRequest, "%KEYWORD%", strSearchKeyword)

If Not fQuiet Then
	WriteOutput "Performing " & cIterations & " iteration(s) ..."
	WriteOutput ""
End If
Dim iIteration, hq, strStatusPrefix
For iIteration = 1 to cIterations

	' Instantiate the CHttpQuery object that will be used to perform the query.
	Set hq = Nothing
	If Err.Number <> 0 Then DisplayErrorSummaryAndExit "Error releasing CHttpQuery object."
	Set hq = New CHttpQuery
	If Err.Number <> 0 Then DisplayErrorSummaryAndExit "Error instantiating CHttpQuery object."
	

	' Start the query.
	g_strStatisticLoggingContext = iIteration & " iteration:"
Err.Clear ' Ignore errors with timers.

	g_tmrLoop.Restart
Err.Clear ' Ignore errors with timers.

	g_tmrStep.Restart
Err.Clear ' Ignore errors with timers.

	hq.StartQueryEx strHttpVerbParam, strDestUrlParam, strRequest, strContentType, nDepth, iRequestRangeStart, iRequestRangeEnd, iRequestMaxRows, dictRequestHeaders
	If Err.Number <> 0 Then DisplayErrorSummaryAndExit "Error starting query."
	g_csecStep = g_tmrStep.ElapsedTime
LogStatistic "Starting query" & " (us)", g_csecStep * 1000000
Err.Clear ' Ignore errors with timers.


	' Block until the results come back.
	hq.WaitForQueryCompletion
	If Err.Number <> 0 Then DisplayErrorSummaryAndExit "Error waiting for the query to complete."
	
	g_csecLoop = g_tmrLoop.ElapsedTime
LogStatistic "Starting query and waiting for results" & " (us)", g_csecLoop * 1000000
Err.Clear ' Ignore errors with timers.


	' Examine the status of the query.
	If (hq.QueryHttpStatus <> HTTPSTATUS_MULTISTATUS) And (hq.QueryHttpStatus <> HTTPSTATUS_OK) Then
		' An error occurred.
		strStatusPrefix = "ERROR: "
	Else
		strStatusPrefix = ""
	End If
	If Not fQuiet Then
		WriteOutput iIteration & ": " & strStatusPrefix & "Response status = " & hq.QueryHttpStatus & ", TotalHits = " & hq.TotalHits
	End If
	If fDumpResponseHeaders Then
		WriteOutput "    " & Replace(hq.QueryResponseHeaders, vbCRLF, vbCRLF & "    ")
	End If
Next
If Not fQuiet Then
	WriteOutput ""
End If

' Tell the user which XMLHTTP object wound up being used.
If Not fQuiet Then
	WriteOutput ""
	If g_fUseNewXmlHttpObject Then
		WriteOutput "The new Msxml2.ServerXMLHTTP.3.0 object was used."
	Else
		WriteOutput "The old Microsoft.XMLHTTP object was used."
	End If
End If

' Tell the user about the elapsed times that were measured.
If Not fQuiet Then
	WriteOutput GetLoggedStatistics()
End If

' Optionally write the results to an output file.
If Len(strResponseOutputFileName) > 0 Then
	' Get the response text.
	Dim strResponseText
	strResponseText = hq.QueryResults
	If Err.Number <> 0 Then DisplayErrorSummaryAndExit "Error getting response text from XMLHTTP object."

	' Strip the encoding="utf-8" header unless that's disabled.
	If fStripEncodingUtf8Header Then
	    strResponseText = Replace(strResponseText, "encoding=""UTF-8""", "")
	End IF

	' Write the modified string to the output file.
	WriteStringToFile strResponseOutputFileName, strResponseText
	If Err.Number <> 0 Then DisplayErrorSummaryAndExit "Error writing response text to " & strResponseOutputFileName
	If Not fQuiet Then
		WriteOutput "Response written to " & strResponseOutputFileName
	End If

	' Optionally spawn IE.
	If fViewInIE Then
		Dim nResult
		nResult = g_wshShell.Run("iexplore """ & strResponseOutputFileName & """")
		If Err.Number <> 0 Then DisplayErrorSummaryAndExit "Error spawning IE."
		If nResult <> 0 Then Err.Raise nResult, "ThisScript", "Error returned from new process.." : DisplayErrorSummaryAndExit "Error spawning IE."
	End If
End If

' Optionally display the results to stdout.
If fListResponseToConsole Then
	' Display the results.
	If Not fQuiet Then
		WriteOutput "Query results:"
		WriteOutput "----------------------------------------"
		WriteOutput hq.QueryResults
	End If
End If

' That's it. We're done.
wscript.Quit



Sub WriteOutput(strOutput)
	On Error Resume Next
    wscript.Echo strOutput
End Sub

Sub ShowUsageAndExit(strMsg)
	On Error Resume Next
	WriteOutput ""
	WriteOutput strMsg
	WriteOutput ""
	WriteOutput ""
	WriteOutput "This script executes an HTTP query using the XMLHTTP object."
	WriteOutput ""
	WriteOutput "Usage: HttpRequest HttpVerb Url [RequestFile]"
	WriteOutput ""
	WriteOutput "Parameters:"
	WriteOutput ""
	WriteOutput "  HttpVerb - Specifies the HTTP verb that you want to invoke. This can be"
	WriteOutput "             one of the following values: GET, PROPFIND"
	WriteOutput ""
	WriteOutput "  Url - Specifies the URL where the request is to be sent."
	WriteOutput ""
	WriteOutput "Optional Parameters and Switches:"
	WriteOutput ""
	WriteOutput "  RequestFile - Specifies a file which will be used as the body of the request."
	WriteOutput ""
	WriteOutput "  /q - Quiet. Don't display any normal output. Only display errors that occur."
	WriteOutput ""
	WriteOutput "  /new - Use the new ServerXMLHTTP object instead of the XMLHTTP object."
	WriteOutput ""
	WriteOutput "  /list - List the response text to stdout."
	WriteOutput ""
	WriteOutput "  /out[:FileName] - Specifies that the response text should be written to a"
	WriteOutput "      file. If no file name is specified, then the following file is used:"
	WriteOutput "          " & strStandardOutputFileTemplate & " is used."
	WriteOutput ""
	WriteOutput "  /nostrip - Disables the stripping of encoding=""utf-8"" from the output file"
	WriteOutput "      which is done by default to make viewing with IE possible."
	WriteOutput ""
	WriteOutput "  /ie - Specifies that IE should be used to view the output file."
	WriteOutput ""
	WriteOutput "  /k:keyword - Specifies the search keyword to replace in the request body."
	WriteOutput "      This is used with either the default query or a query specified using the"
	WriteOutput "      RequestFile parameter where the string %KEYWORD% is found in the request."
	WriteOutput ""
	WriteOutput "  /c:num - Performs the HTTP request the number of times specified."
	WriteOutput ""
	WriteOutput "  /d - Displays all of the response headers."
	WriteOutput ""
	WriteOutput "  /h:name[=value] - Specifies a request header name and value to be sent."
	WriteOutput ""
	WriteOutput "  /b:nn - Specifies the beginning of the row range to request (SEARCH request"
	WriteOutput "      only)."
	WriteOutput ""
	WriteOutput "  /e:nn - Specifies the end of the row range to request (SEARCH request only)."
	WriteOutput ""
	WriteOutput "  /x:nn - Specifies the maximum number of rows to scan (SEARCH request only)."
	WriteOutput ""
	WriteOutput "  /u:username - Specifies an overridden user name."
	WriteOutput ""
	WriteOutput "  /p:password - Specifies an overridden password."
	WriteOutput ""
	WriteOutput "Notes:"
	WriteOutput ""
	WriteOutput "  For the PROPFIND verb, there is a default body that is use if the RequestFile"
	WriteOutput "  is not specified which does a DAV:propall with a depth of 1."
	WriteOutput ""
	WriteOutput "  For the SEARCH verb, there is a default body that is used if the RequestFile"
	WriteOutput "  is not specified which does a standard Tahoe Portal DEEP TRAVERSAL search. You"
	WriteOutput "  can specify the search keyword using the optional /k switch."
	WriteOutput ""
	WriteOutput "Examples:"
	WriteOutput "  HttpRequest GET http://foo/bar.doc"
	WriteOutput "  HttpRequest PROPFIND http://localhost/vroot"
	WriteOutput "  HttpRequest SEARCH http://server/vroot ShallowSearch.txt /out:f:\foo.xml"
	WriteOutput "  HttpRequest SEARCH http://server/vroot /out /ie /k:filbert"
	WriteOutput ""

	wscript.Quit
End Sub
