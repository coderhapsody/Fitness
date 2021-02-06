<%@ Application Language="C#" %>
<%@ Import Namespace="Microsoft.Practices.Unity" %>
<%@ Import Namespace="FitnessManagement.Data" %>

<script runat="server">

    void Application_BeginRequest(object sender, EventArgs e)
    {
    }

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup        
        ApplicationLogger.Write("Application is starting " + HttpRuntime.AppDomainAppVirtualPath);

        IUnityContainer container = new UnityContainer();

        container.RegisterType<FitnessManagement.Data.FitnessDataContext>(
            new HttpContextLifetimeManager<FitnessManagement.Data.FitnessDataContext>(), 
            new InjectionConstructor());
        
        UnityContainerHelper.Container = container;       
    }   

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
        ApplicationLogger.Write("Application has ended " + HttpRuntime.AppDomainAppVirtualPath);        
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Exception ex = Server.GetLastError().GetBaseException();
        ApplicationLogger.Write("Application Error: " + ex.Message);
        ApplicationLogger.Write("Application Error: " + ex.Message, "Exception", System.Diagnostics.TraceEventType.Critical);
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    protected void Application_EndRequest(object sender, EventArgs e)
    {
        var dataContext = HttpContext.Current.Items.Values.OfType<FitnessDataContext>().SingleOrDefault();
        if (dataContext != null)
            dataContext.Dispose();
    }
</script>
