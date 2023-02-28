using HESDanfe.Graphics;
using System;

namespace HESDanfe
{
    /// <summary>
    /// Elemento básico no DANFE.
    /// </summary>
    internal abstract class ElementoBase : DrawableBase
    {
        public Estilo Estilo { get; protected set; }
        public virtual bool PossuiContorno => true;

        public ElementoBase(Estilo estilo)
        {
            Estilo = estilo ?? throw new ArgumentNullException(nameof(estilo));
        }

        public override void Draw(Gfx gfx)
        {
            base.Draw(gfx);
            if (PossuiContorno)
                gfx.StrokeRectangle(BoundingBox, 0.25f);
        }
    }
}
