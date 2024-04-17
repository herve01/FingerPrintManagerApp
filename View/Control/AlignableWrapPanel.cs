using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FingerPrintManagerApp.View.Control
{
    //public class AlignableWrapPanel : Panel
    //{
    //    public HorizontalAlignment HorizontalContentAlignment
    //    {
    //        get { return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty); }
    //        set { SetValue(HorizontalContentAlignmentProperty, value); }
    //    }

    //    public static readonly DependencyProperty HorizontalContentAlignmentProperty =
    //        DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(AlignableWrapPanel), new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.AffectsArrange));

    //    protected override Size MeasureOverride(Size constraint)
    //    {
    //        Size curLineSize = new Size();
    //        Size panelSize = new Size();

    //        UIElementCollection children = base.InternalChildren;

    //        for (int i = 0; i < children.Count; i++)
    //        {
    //            UIElement child = children[i] as UIElement;

    //            //Flow passes its own constraint to children
    //            child.Measure(constraint);
    //            Size sz = child.DesiredSize;

    //            if (curLineSize.Width + sz.Width > constraint.Width) //need to switch to another line
    //            {
    //                panelSize.Width = Math.Max(curLineSize.Width, panelSize.Width);
    //                panelSize.Height += curLineSize.Height;
    //                curLineSize = sz;

    //                if (sz.Width > constraint.Width) // if the element is wider then the constraint - give it a separate line                    
    //                {
    //                    panelSize.Width = Math.Max(sz.Width, panelSize.Width);
    //                    panelSize.Height += sz.Height;
    //                    curLineSize = new Size();
    //                }
    //            }
    //            else //continue to accumulate a line
    //            {
    //                curLineSize.Width += sz.Width;
    //                curLineSize.Height = Math.Max(sz.Height, curLineSize.Height);
    //            }
    //        }

    //        //the last line size, if any need to be added
    //        panelSize.Width = Math.Max(curLineSize.Width, panelSize.Width);
    //        panelSize.Height += curLineSize.Height;

    //        return panelSize;
    //    }

    //    protected override Size ArrangeOverride(Size arrangeBounds)
    //    {
    //        int firstInLine = 0;
    //        Size curLineSize = new Size();
    //        double accumulatedHeight = 0;
    //        UIElementCollection children = this.InternalChildren;

    //        for (int i = 0; i < children.Count; i++)
    //        {
    //            Size sz = children[i].DesiredSize;

    //            if (curLineSize.Width + sz.Width > arrangeBounds.Width) //need to switch to another line
    //            {
    //                ArrangeLine(accumulatedHeight, curLineSize, arrangeBounds.Width, firstInLine, i);

    //                accumulatedHeight += curLineSize.Height;
    //                curLineSize = sz;

    //                if (sz.Width > arrangeBounds.Width) //the element is wider then the constraint - give it a separate line                    
    //                {
    //                    ArrangeLine(accumulatedHeight, sz, arrangeBounds.Width, i, ++i);
    //                    accumulatedHeight += sz.Height;
    //                    curLineSize = new Size();
    //                }
    //                firstInLine = i;
    //            }
    //            else //continue to accumulate a line
    //            {
    //                curLineSize.Width += sz.Width;
    //                curLineSize.Height = Math.Max(sz.Height, curLineSize.Height);
    //            }
    //        }

    //        if (firstInLine < children.Count)
    //            ArrangeLine(accumulatedHeight, curLineSize, arrangeBounds.Width, firstInLine, children.Count);

    //        return arrangeBounds;
    //    }

    //    private void ArrangeLine(double y, Size lineSize, double boundsWidth, int start, int end)
    //    {
            
    //        double x = 0;
    //        if (this.HorizontalContentAlignment == HorizontalAlignment.Center)
    //        {
    //            x = (boundsWidth - lineSize.Width) / 2;
    //        }
    //        else if (this.HorizontalContentAlignment == HorizontalAlignment.Right)
    //        {
    //            x = (boundsWidth - lineSize.Width);
    //        }

    //        UIElementCollection children = InternalChildren;
    //        for (int i = start; i < end; i++)
    //        {
    //            UIElement child = children[i];
    //            child.Arrange(new Rect(x, y, child.DesiredSize.Width, lineSize.Height));
    //            x += child.DesiredSize.Width;
    //        }
    //    }

    //}

    public class AlignableWrapPanel : Panel
    {

        /// <summary>
        /// <see cref="HorizontalAlignment"/> property definition.
        /// </summary>
        public static readonly DependencyProperty HorizontalContentAlignmentProperty =
            DependencyProperty.Register(
                "HorizontalContentAlignment",
                typeof(HorizontalAlignment),
                typeof(AlignableWrapPanel),
                new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.AffectsArrange)
            );

        /// <summary>
        /// Gets or sets the horizontal alignment of the control's content.
        /// </summary>
        [BindableAttribute(true)]
        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Measures the size in layout required for child elements and determines a size for the <see cref="AlignableWrapPanel"/>.
        /// </summary>
        /// <param name="constraint">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            var curLineSize = new Size();
            var panelSize = new Size();
            var children = base.InternalChildren;
            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i] as UIElement;
                // Flow passes its own constraint to children
                child.Measure(constraint);

                var sz = child.DesiredSize;
                if (curLineSize.Width + sz.Width > constraint.Width)
                { //need to switch to another line
                    panelSize.Width = Math.Max(curLineSize.Width, panelSize.Width);
                    panelSize.Height += curLineSize.Height;
                    curLineSize = sz;
                    if (sz.Width > constraint.Width)
                    { // if the element is wider then the constraint - give it a separate line                    
                        panelSize.Width = Math.Max(sz.Width, panelSize.Width);
                        panelSize.Height += sz.Height;
                        curLineSize = new Size();
                    }
                }
                else
                { //continue to accumulate a line
                    curLineSize.Width += sz.Width;
                    curLineSize.Height = Math.Max(sz.Height, curLineSize.Height);
                }
            }
            // the last line size, if any need to be added
            panelSize.Width = Math.Max(curLineSize.Width, panelSize.Width);
            panelSize.Height += curLineSize.Height;
            return panelSize;
        }

        /// <summary>
        /// Positions child elements and determines a size for a <see cref="AlignableWrapPanel"/>.
        /// </summary>
        /// <param name="arrangeBounds">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var firstInLine = 0;
            var curLineSize = new Size();
            var accumulatedHeight = 0.0;
            var children = InternalChildren;
            //var maxLineWidth = 0.0;

            for (var i = 0; i < children.Count; i++)
            {
                var sz = children[i].DesiredSize;
                if (curLineSize.Width + sz.Width > arrangeBounds.Width)
                { //need to switch to another line
                    //maxLineWidth = Math.Max(curLineSize.Width, maxLineWidth);

                    ArrangeLine(accumulatedHeight, curLineSize, arrangeBounds.Width, firstInLine, i);
                    accumulatedHeight += curLineSize.Height;
                    curLineSize = sz;
                    if (sz.Width > arrangeBounds.Width)
                    { //the element is wider then the constraint - give it a separate line                    
                        ArrangeLine(accumulatedHeight, sz, arrangeBounds.Width, i, ++i);
                        accumulatedHeight += sz.Height;
                        curLineSize = new Size();
                    }
                    firstInLine = i;
                }
                else
                { //continue to accumulate a line
                    curLineSize.Width += sz.Width;
                    curLineSize.Height = Math.Max(sz.Height, curLineSize.Height);
                }
            }

            if (firstInLine < children.Count)
                ArrangeLine(accumulatedHeight, curLineSize, arrangeBounds.Width, firstInLine, children.Count);
            return arrangeBounds;
        }

        /// <summary>
        /// Arranges elements within a line.
        /// </summary>
        /// <param name="y">Line vertical coordinate.</param>
        /// <param name="lineSize">Size of the items line.</param>
        /// <param name="boundsWidth">Width of the panel bounds.</param>
        /// <param name="start">Index of the first child which belongs to the line.</param>
        /// <param name="end">Index of the last child which belongs to the line.</param>
        private void ArrangeLine(double y, Size lineSize, double boundsWidth, int start, int end)
        {
            var children = InternalChildren;
            var x = 0.0;
            var stretchOffset = 0.0;
            if (HorizontalContentAlignment == HorizontalAlignment.Center)
            {
                //x = (boundsWidth - lineSize.Width) / 2;
                var childWidth = children[start].DesiredSize.Width; // warning, this works only when all children have equal widths
                int n = (int)boundsWidth / (((int)childWidth) == 0 ? 1 : ((int)childWidth));
                n = n > children.Count ? children.Count : n;
                //if (children.Count > n)
                //{
                //    var takenWidth = n * childWidth;
                //    var spaceWidth = boundsWidth - takenWidth;
                //    stretchOffset = spaceWidth / 2;
                //}

                var takenWidth = n * childWidth;
                var spaceWidth = boundsWidth - takenWidth;
                stretchOffset = spaceWidth / 2;

                for (var i = start; i < end; i++)
                {
                    if (i == start)
                        x = stretchOffset;

                    var child = children[i];
                    child.Arrange(new Rect(x, y, child.DesiredSize.Width, lineSize.Height));
                    x += child.DesiredSize.Width; // + stretchOffset;
                }
            }
            else if (HorizontalContentAlignment == HorizontalAlignment.Right)
            {
                x = (boundsWidth - lineSize.Width);
                for (var i = start; i < end; i++)
                {
                    var child = children[i];
                    child.Arrange(new Rect(x, y, child.DesiredSize.Width, lineSize.Height));
                    x += child.DesiredSize.Width + stretchOffset;
                }
            }
            else if (HorizontalAlignment == HorizontalAlignment.Stretch)
            {
                var childWidth = children[start].DesiredSize.Width; // warning, this works only when all children have equal widths
                int n = (int)boundsWidth / (((int)childWidth) == 0 ? 1 : ((int)childWidth));
                //if (children.Count > n)
                //{
                //    var takenWidth = n * childWidth;
                //    var spaceWidth = boundsWidth - takenWidth;
                //    stretchOffset = spaceWidth / n;
                //}

                var takenWidth = n * childWidth;
                var spaceWidth = boundsWidth - takenWidth;
                spaceWidth = spaceWidth > takenWidth / 2 ? 0 : spaceWidth;
                stretchOffset = spaceWidth / n;

                for (var i = start; i < end; i++)
                {
                    if (i == start)
                        x = stretchOffset / 2;

                    var child = children[i];
                    child.Arrange(new Rect(x, y, child.DesiredSize.Width, lineSize.Height));
                    x += child.DesiredSize.Width + stretchOffset;
                }
            }
            
        }

    }
}