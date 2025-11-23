using System;

namespace OrderPipeline
{
 // EventArgs for shipping event
 public class ShipEventArgs : EventArgs
 {
 public string Product { get; }
 public bool Express { get; }

 public ShipEventArgs(string product, bool express)
 {
 Product = product;
 Express = express;
 }
 }
}
