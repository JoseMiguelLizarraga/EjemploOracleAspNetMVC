//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class PRODUCTO
    {
        public decimal PRODUCTO_ID { get; set; }
        public string NOMBRE_PRODUCTO { get; set; }
        public Nullable<decimal> PRECIO_PRODUCTO { get; set; }
        public Nullable<decimal> STOCK_PRODUCTO { get; set; }
        public string IMAGEN_PRODUCTO { get; set; }
        public Nullable<decimal> CATEGORIA_ID { get; set; }
    
        public virtual CATEGORIA CATEGORIA { get; set; }
    }
}
