//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class user_to_group
    {
        public int id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> group_id { get; set; }
        public Nullable<bool> is_manager { get; set; }
    
        public virtual group group { get; set; }
        public virtual user user { get; set; }
    }
}
