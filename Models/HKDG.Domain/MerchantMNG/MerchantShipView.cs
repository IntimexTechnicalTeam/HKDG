﻿namespace HKDG.Domain
{
    public class MerchantShipView
    {

        public Guid Id { get; set; }

        public string SPName { get; set; }

        public string SPPassword { get; set; }

        public string SPIntegraterName { get; set; }


        public string ECShipName { get; set; }

        public string ECShipPassword { get; set; }

        public string ECShipIntegraterName { get; set; }

        public string ECShipEmail { get; set; }
    }
}
