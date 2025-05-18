namespace API.Entity.Code
{
    public class ProcessBrokerLoginEntity()
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string iv_username { get; set; }
        public string iv_password { get; set; }
        public ClientMetadata client_metadata { get; set; }
    }

    public class ClientMetadata
    {
        public string browser { get; set; }
        public string ip_address { get; set; }
        public string accept_language { get; set; }
        public string referer { get; set; }
        public string host { get; set; }
        public string request_method { get; set; }
        public string query_string { get; set; }
        public object session_key { get; set; }
        public Cookies cookies { get; set; }
    }

    public class Cookies
    {
        public string csrftoken { get; set; }
    }
}
