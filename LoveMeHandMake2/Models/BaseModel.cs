using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public abstract class BaseModel
    {
        protected static ILog log = LogManager.GetLogger(typeof(BaseModel));

        [Key]
        [Column(Order = 1)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [JsonIgnore]
        public DateTime CreateTime { get; set; }

        [Required]
        [JsonIgnore]
        public DateTime UpdateTime { get; set; }

        [Required]
        [JsonIgnore]
        public bool ValidFlag { get; set; }

        public virtual void Create()
        {
            this.CreateTime = System.DateTime.Now;
            this.UpdateTime = System.DateTime.Now;
            this.ValidFlag = true;
        }

        public virtual void Update()
        {
            this.UpdateTime = System.DateTime.Now;
        }

        public virtual void Delete()
        {
            this.UpdateTime = System.DateTime.Now;
            this.ValidFlag = false;
        }

    }
}