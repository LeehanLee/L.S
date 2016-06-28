using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Model.DatabaseModel.Entity
{
    public class Info : BaseEntity
    {
        /// <summary>
        /// 资讯简介/导读
        /// </summary>
        [MaxLength(200)]
        public string Introduction { get; set; }

        /// <summary>
        /// 图标路径
        /// </summary>
        [MaxLength(2000)]
        public string ImgPath { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [MaxLength(100)]
        public string Source { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [MaxLength(10)]
        public string Author { get; set; }
        public int ViewCount { get; set; }
        public int CommontCount { get; set; }

        /// <summary>
        /// 所属分类ID
        /// </summary>
        [MaxLength(36)]
        public string CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
    }
}
