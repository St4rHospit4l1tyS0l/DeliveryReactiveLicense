﻿namespace Infrastructure.Models
{
    public class ResponseMessageModel
    {
        public bool HasError { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}
