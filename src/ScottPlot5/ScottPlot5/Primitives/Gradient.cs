﻿namespace ScottPlot;

public class Gradient(GradiantType gradientType = GradiantType.Linear) : IHatch
{
    /// <summary>
    /// Describes the geometry of a color gradient used to fill an area
    /// </summary>
    public GradiantType GradiantType { get; set; } = gradientType;

    /// <summary>
    /// Get or set the start angle in degrees for sweep gradient
    /// </summary>
    public float StartAngle { get; set; } = 0f;

    /// <summary>
    /// Get or set the end angle in degrees for sweep gradient
    /// </summary>
    public float EndAngle { get; set; } = 360f;

    /// <summary>
    /// Get or set how the shader should handle drawing outside the original bounds.
    /// </summary>
    public SKShaderTileMode TileMode { get; set; } = SKShaderTileMode.Clamp;

    /// <summary>
    /// Start of linear gradient
    /// </summary>
    public Alignment AlignmentStart { get; set; } = Alignment.UpperLeft;

    /// <summary>
    /// End of linear gradient
    /// </summary>
    public Alignment AlignmentEnd { get; set; } = Alignment.LowerRight;

    public SKShader GetShader(Color backgroundColor, Color hatchColor, PixelRect rect)
    {
        return GradiantType switch
        {
            GradiantType.Radial => SKShader.CreateRadialGradient(
                center: new SKPoint(rect.HorizontalCenter, rect.VerticalCenter),
                radius: Math.Max(rect.Width, rect.Height) / 2.0f,
                colors: [backgroundColor.ToSKColor(), hatchColor.ToSKColor()],
                mode: TileMode
                ),

            GradiantType.Sweep => SKShader.CreateSweepGradient(
                center: new SKPoint(rect.HorizontalCenter, rect.VerticalCenter),
                colors: [backgroundColor.ToSKColor(), hatchColor.ToSKColor()],
                colorPos: null,
                tileMode: TileMode,
                startAngle: StartAngle,
                endAngle: EndAngle
                ),

            GradiantType.TwoPointConical => SKShader.CreateTwoPointConicalGradient(
                start: rect.TopLeft.ToSKPoint(),
                startRadius: Math.Min(rect.Width, rect.Height),
                end: rect.BottomRight.ToSKPoint(),
                endRadius: Math.Min(rect.Width, rect.Height),
                colors: [backgroundColor.ToSKColor(), hatchColor.ToSKColor()],
                colorPos: null,
                mode: TileMode
                ),

            _ => SKShader.CreateLinearGradient(
                start: rect.GetAlignedPixel(AlignmentStart).ToSKPoint(),
                end: rect.GetAlignedPixel(AlignmentEnd).ToSKPoint(),
                colors: [backgroundColor.ToSKColor(), hatchColor.ToSKColor()],
                mode: TileMode
                ),
        };
    }
}
