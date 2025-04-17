namespace BookTour.Dto.Request
{
    public class BookTourRequest
    {
        public int TourId { get; set; } 
        public int UserId { get; set; }  
        public DateTime TourDate { get; set; }  
        public int AdultCount { get; set; } 
        public int ChildCount { get; set; } 
        public string PaymentMethod { get; set; } 
        public List<PassengerRequest> Passengers { get; set; }  
    }

    public class PassengerRequest
    {
        public string PassengerName { get; set; } 
        public string PassengerType { get; set; }  
    }
}