namespace MCS.Email.Web.Middlewares
{
    public static class DiMiddleware
    {
        public static void AddMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
    
}
