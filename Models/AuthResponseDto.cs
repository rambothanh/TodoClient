namespace TodoClient.Models {
    public class AuthResponseDto {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    
        public string Token { get; set; }
        public string Message { get; set; }
        public bool IsAuthSuccessful { get; set; }

    }
   
}