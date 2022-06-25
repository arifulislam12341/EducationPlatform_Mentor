
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace EducationPlatform.Models
{

using System;
    using System.Collections.Generic;
    
public partial class Cours
{

    public Cours()
    {

        this.Carts = new HashSet<Cart>();

        this.CourseDetails = new HashSet<CourseDetail>();

        this.Notices = new HashSet<Notice>();

        this.Ratings = new HashSet<Rating>();

        this.Transactions = new HashSet<Transaction>();

        this.ValidStudents = new HashSet<ValidStudent>();

    }


    public int Id { get; set; }

    public string Name { get; set; }

    public string Details { get; set; }

    public Nullable<double> Price { get; set; }

    public string Duration { get; set; }

    public Nullable<System.DateTime> Date { get; set; }

    public Nullable<int> MentorId { get; set; }



    public virtual ICollection<Cart> Carts { get; set; }

    public virtual ICollection<CourseDetail> CourseDetails { get; set; }

    public virtual Mentor Mentor { get; set; }

    public virtual ICollection<Notice> Notices { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; }

    public virtual ICollection<ValidStudent> ValidStudents { get; set; }

}

}
