# KlxPiao.ColorTool

A simple and easy-to-use color space conversion tool that supports mutual conversion between HSL/HSV and RGB color spaces.  

## Installation

```bash  
Install-Package KlxPiao.ColorTool  
```  

## Usage (Similarly for Hsv)

#### 1️⃣ **Convert HSL to Color**
```csharp
// Method 1: Implicit conversion via HslColor
var hsl = HslColor.FromHsl(0, 1, 1);    // Using float parameters (H: 0-360°, S/L: 0-1)
Color color1 = hsl;                     // Implicit conversion to Color

// Method 2: Direct creation from string
var hsl2 = HslColor.FromString("0, 100%, 100%"); 
Color color2 = hsl2;                    // Implicit conversion to Color

// Method 3: Quick methods (no explicit instantiation)
Color color3 = HslColor.FromHsl(0, 1, 1);
Color color4 = HslColor.FromString("0, 100%, 100%");
```

#### 2️⃣ **Convert Color to HSL**
```csharp
// Get HSL values via implicit conversion
HslColor hsl = Colors.Red;  // Implicit conversion to HslColor
float h = hsl.Hue;          // Output: 0f (0-360 range)
float s = hsl.Saturation;   // Output: 1f (0-1 range)
float l = hsl.Lightness;    // Output: 1f (0-1 range)

// Get string representation
string hslString1 = hsl.ToString();           // Output: "0, 100%, 100%"

// Get string via extension method
string hslString2 = Colors.Red.ToHslString(); // Output: "0, 100%, 100%"
```
---
### 📝 **Notes**
1. **Parameter Ranges**:
   - `FromHsl()` accepts `Hue: 0-360`, `Saturation/Lightness: 0-1`
   - `FromString()` requires format: `"H, S%, L%"`
2. **Compatibility**:
   - Supports both spaced (e.g., `"0, 100%, 50%"`) and unspaced (e.g., `"0,100%,50%"`) formats
   - Parameters are automatically clamped to valid ranges (e.g., 1.5 → 1.0, 721 → 1)
   - When S/L values include '%', the symbol is removed and divided by 100. Without '%', values remain unchanged.
3. **Implicit Conversion Equivalence**:
   - Implicit conversion is equivalent to `color.ToHsl()` or `hslColor.ToColor()`
4. **Exception Handling**:
   - Invalid strings throw `FormatException`