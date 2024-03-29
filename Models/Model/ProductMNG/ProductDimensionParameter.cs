﻿namespace Model
{

    public class ProductDimensionParameter : IDimensions
    {

        public ProductDimensionParameter()
        {
            this.Width = 0;
            this.Heigth = 0;
            this.Length = 0;
            this.Unit = -1;
        }

        [Column("Width")]
        public decimal Width { get; set; }

        [Column("Heigth")]
        public decimal Heigth { get; set; }

        [Column("Length")]
        public decimal Length { get; set; }

        [Column("Unit")]
        public int Unit { get; set; }
    }

}
