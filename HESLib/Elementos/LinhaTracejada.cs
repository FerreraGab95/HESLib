﻿using System;
using System.Drawing;
using Extensions;
using HES.Graphics;

namespace HES
{
    internal class LinhaTracejada : DrawableBase
    {
        public float Margin { get; set; }
        public double[] DashPattern { get; set; }

        public LinhaTracejada(float margin)
        {
            Margin = margin;
        }

        public override void Draw(Gfx gfx)
        {
            base.Draw(gfx);

            gfx.PrimitiveComposer.BeginLocalState();
            gfx.PrimitiveComposer.SetLineDash(new HES.Documents.Contents.LineDash(new double[] { 3, 2 }));
            gfx.PrimitiveComposer.DrawLine(new PointF(BoundingBox.Left, Y + Margin).ToPointMeasure(), new PointF(BoundingBox.Right, Y + Margin).ToPointMeasure());
            gfx.PrimitiveComposer.Stroke();
            gfx.PrimitiveComposer.End();

        }

        public override float Height { get => 2 * Margin; set => throw new NotSupportedException(); }
    }
}
