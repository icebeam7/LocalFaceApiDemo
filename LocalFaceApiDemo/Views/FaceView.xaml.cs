using System;

using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using LocalFaceApiDemo.Models;
using LocalFaceApiDemo.ViewModels;

namespace LocalFaceApiDemo.Views
{
    public partial class FaceView : ContentPage
    {
        const float radius = 2.0f;

        public FaceView()
        {
            InitializeComponent();

            var vm = (FaceViewModel)BindingContext;
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(FaceViewModel.Image) || e.PropertyName == nameof(vm.FaceInfo))
                {
                    ImageCanvas.InvalidateSurface();
                }
            };
        }

        void ImageCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var vm = (FaceViewModel)BindingContext;

            var info = e.Info;
            var canvas = e.Surface.Canvas;

            ClearCanvas(info, canvas);

            if (vm.Image != null)
            {
                var scale = Math.Min((float)info.Width / (float)vm.Image.Width, (float)info.Height / (float)vm.Image.Height);

                var scaleHeight = scale * vm.Image.Height;
                var scaleWidth = scale * vm.Image.Width;

                var top = (info.Height - scaleHeight) / 2;
                var left = (info.Width - scaleWidth) / 2;

                canvas.DrawBitmap(vm.Image, new SKRect(left, top, left + scaleWidth, top + scaleHeight));
                //DrawBorder(canvas, left, top, scaleWidth, scaleHeight);
                DrawPredictions(vm, canvas, left, top, scaleWidth, scaleHeight, info);
            }
        }

        static void ClearCanvas(SKImageInfo info, SKCanvas canvas)
        {
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.LightGray
            };

            canvas.DrawRect(info.Rect, paint);
        }

        static void DrawPredictions(FaceViewModel vm, SKCanvas canvas, float left, float top, float scaleWidth, float scaleHeight, SKImageInfo info)
        {
            if (vm.FaceInfo == null) return;

            if (vm.FaceInfo.FaceRectangle != null)
                LabelPrediction(canvas, vm.FaceInfo.MaxEmotion, vm.FaceInfo.FaceRectangle, left, top, scaleWidth, scaleHeight, vm, info);
        }

        static void LabelPrediction(SKCanvas canvas, string tag, FaceRectangle rectangle, float left, float top, float width, float height, FaceViewModel vm, SKImageInfo info, bool addBox = true)
        {
            var scale = Math.Min((float)info.Width / (float)vm.Image.Width, (float)info.Height / (float)vm.Image.Height);

            var scaleHeight = rectangle.Height * scale;
            var scaleWidth = rectangle.Width * scale;

            var scaleTop = rectangle.Top * scale;
            var scaleLeft = rectangle.Left * scale;

            if (addBox)
                DrawBox(canvas, (float)scaleLeft, (float)scaleTop, (float)scaleWidth, (float)scaleHeight);

            DrawText(canvas, tag, left, top, width, height);
        }

        static void DrawText(SKCanvas canvas, string tag, float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var textPaint = new SKPaint
            {
                IsAntialias = true,
                Color = SKColors.White,
                Style = SKPaintStyle.Fill,
                Typeface = SKTypeface.FromFamilyName("Arial")
            };

            var text = tag;

            var textWidth = textPaint.MeasureText(text);
            textPaint.TextSize = 0.5f * scaledBoxWidth * textPaint.TextSize / textWidth;

            var textBounds = new SKRect();
            textPaint.MeasureText(text, ref textBounds);

            var xText = (startLeft + (scaledBoxWidth / 2)) - textBounds.MidX;
            var yText = (startTop + (scaledBoxHeight / 2)) + textBounds.MidY;

            var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = new SKColor(0, 0, 0, 120)
            };

            var backgroundRect = textBounds;
            backgroundRect.Offset(xText, yText);
            backgroundRect.Inflate(10, 10);

            canvas.DrawRoundRect(backgroundRect, 5, 5, paint);

            canvas.DrawText(text,
                            xText,
                            yText,
                            textPaint);
        }

        static void DrawBox(SKCanvas canvas, float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var strokePaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Blue,
                StrokeWidth = 10,
                PathEffect = SKPathEffect.CreateDash(new[] { 20f, 20f }, 20f)
            };
            DrawBox(canvas, strokePaint, startLeft, startTop, scaledBoxWidth, scaledBoxHeight);

            var blurStrokePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
                PathEffect = SKPathEffect.CreateDash(new[] { 20f, 20f }, 20f),
                IsAntialias = true,
                MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, 0.57735f * radius + 0.5f)
            };
            DrawBox(canvas, blurStrokePaint, startLeft, startTop, scaledBoxWidth, scaledBoxHeight);
        }

        static void DrawBorder(SKCanvas canvas, float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var strokePaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.DarkSlateBlue,
                StrokeWidth = 5
            };

            DrawBox(canvas, strokePaint, startLeft, startTop, scaledBoxWidth, scaledBoxHeight);
        }

        static void DrawBox(SKCanvas canvas, SKPaint paint, float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var path = CreateBoxPath(startLeft, startTop, scaledBoxWidth, scaledBoxHeight);
            canvas.DrawPath(path, paint);
        }

        static SKPath CreateBoxPath(float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var path = new SKPath();
            path.MoveTo(startLeft, startTop);

            path.LineTo(startLeft + scaledBoxWidth, startTop);
            path.LineTo(startLeft + scaledBoxWidth, startTop + scaledBoxHeight);
            path.LineTo(startLeft, startTop + scaledBoxHeight);
            path.LineTo(startLeft, startTop);

            return path;
        }
    }
}
