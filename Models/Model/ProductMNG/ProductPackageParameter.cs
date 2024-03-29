﻿namespace Model
{
    public class ProductPackageParameter : IDimensions
    {
        public ProductPackageParameter()
        {
            this.Width = 0;
            this.Heigth = 0;
            this.Length = 0;
            this.Unit = -1;
        }

        [Column("PackageWidth")]
        public decimal Width { get; set; }

        [Column("PackageHeigth")]
        public decimal Heigth { get; set; }

        [Column("PackageLength")]
        public decimal Length { get; set; }

        [Column("PackageUnit")]
        public int Unit { get; set; }
    }
}
