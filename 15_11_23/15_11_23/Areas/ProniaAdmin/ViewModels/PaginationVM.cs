﻿namespace _15_11_23.Areas.ProniaAdmin.ViewModels
{
    public class PaginationVM<T> where T : class, new() 
    {
        public int CurrentPage { get; set; }
        public double TotalPage { get; set; }
        public List<T> Item { get; set; }
    }
}
