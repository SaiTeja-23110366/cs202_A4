using System;

namespace LAB11
{
 public class ColorEventArgs : EventArgs
 {
 public string ColorName { get; }

 public ColorEventArgs(string colorName)
 {
 ColorName = colorName;
 }
 }
}
