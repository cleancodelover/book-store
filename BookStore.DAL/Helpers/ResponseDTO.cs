using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL.Helpers
{
    public class ResponseDTO
    {
        public string? Message { get; set; }
        public int Status { get; set; }
        public object? Data { get; set; }
    }
}
