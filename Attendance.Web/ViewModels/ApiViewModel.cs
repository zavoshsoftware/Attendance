using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// مدل برگشتی سفارشی شده در برنامه
/// </summary>
public class CustomResponseViewModel
{
    public CustomResponseViewModel()
    {
        Messages = new List<MessageViewModel>();
        Extra = new object();
    }
    /// <summary>
    /// آیا درخواست به درستی انجام شده است
    /// </summary>
    public bool Ok { get; set; }
    /// <summary>
    /// پیام هایی که برنامه برمیگرداند
    /// </summary>
    public List<MessageViewModel> Messages { get; set; }
    /// <summary>
    /// اطلاعات اضافی که توسط درخواست کننده خواسته شده است
    /// </summary>
    public object Extra { get; set; }
}


/// <summary>
/// مدل نمایشی پیغام ها در مدل برگشتی سفارشی
/// </summary>
public class MessageViewModel
{
    /// <summary>
    /// شناسه اتفاقی که افتاده / خطا یا مورد انجام شده
    /// </summary>
    public int Code { get; set; }
    /// <summary>
    /// توضیحات
    /// </summary>
    public string Description { get; set; }
}