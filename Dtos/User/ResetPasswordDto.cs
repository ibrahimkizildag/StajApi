namespace Api.Dtos.User
{
    public class ResetPasswordDto
    {
        public string Username { get; set; }
        public string Safeword { get; set; }
        public string NewPassword { get; set; }
    }

}
