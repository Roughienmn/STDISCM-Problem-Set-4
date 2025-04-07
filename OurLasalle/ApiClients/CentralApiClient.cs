namespace OurLasalle.ApiClients
{
    public class CentralApiClient
    {
        public AuthServiceClient AuthService { get; }
        //public GradesServiceClient GradesService { get; }
        //public CoursesServiceClient CoursesService { get; }

        public CentralApiClient(AuthServiceClient authService)//, GradesServiceClient gradesService, CoursesServiceClient coursesService)
        {
            AuthService = authService;
            //GradesService = gradesService;
            //CoursesService = coursesService;
        }
    }
}
