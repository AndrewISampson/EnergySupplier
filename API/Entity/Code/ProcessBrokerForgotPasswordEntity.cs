namespace API.Entity.Code
{
    public class ProcessBrokerForgotPasswordEntity()
    {
        public string Username { get; set; }
        public string iv_username { get; set; }
        public string Step { get; set; }
        public string ValidationCode { get; set; }
        public string iv_ValidationCode { get; set; }
        public string Password { get; set; }
        public string iv_Password { get; set; }
    }
}
