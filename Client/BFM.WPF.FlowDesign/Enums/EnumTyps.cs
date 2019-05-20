namespace BFM.WPF.FlowDesign.Enums
{
    /// <summary>
    /// 拖拽方向
    /// </summary>
    public enum EmDragDirection
    {
        /// <summary>
        /// 左上角
        /// </summary>
        TopLeft = 1,
        /// <summary>
        /// 中间上部
        /// </summary>
        TopCenter = 2,
        TopRight = 4,
        MiddleLeft = 16,
        MiddleCenter = 32,
        MiddleRight = 64,
        BottomLeft = 256,
        BottomCenter = 512,
        BottomRight = 1024,
    }

    /// <summary>
    /// 流程控件类型
    /// </summary>
    public enum EmFlowCtrlType
    {
        None = 0,
        /// <summary>
        /// 图形
        /// </summary>
        Image = 1,
        /// <summary>
        /// 直线类形状
        /// </summary>
        PolygonSharp = 2,
        /// <summary>
        /// 圆形空间
        /// </summary>
        CircleSharp = 3,
        /// <summary>
        /// 视频-摄像头
        /// </summary>
        Video = 4,
        Container = 10,
    }

    /// <summary>
    /// 标准形状
    /// </summary>
    public enum EmBasicShape
    {
        /// <summary>
        /// 非标准形状
        /// </summary>
        None = 0,
        /// <summary>
        /// 圆
        /// </summary>
        Ellipse = 2,
        /// <summary>
        /// 三角形
        /// </summary>
        Triangle = 3,
        /// <summary>
        /// 长方形
        /// </summary>
        Rectangle = 4,
        /// <summary>
        /// 五边形
        /// </summary>
        Pentagon = 5,
        /// <summary>
        /// 六边形
        /// </summary>
        Hexagon = 6,
        /// <summary>
        /// 五角星
        /// </summary>
        Star5 = 7,
        /// <summary>
        /// 菱形
        /// </summary>
        Diamond = 8,
        /// <summary>
        /// 平行四边形
        /// </summary>
        Parallelogram = 9,
        /// <summary>
        /// 箭头
        /// </summary>
        Arrow = 10,
        /// <summary>
        /// 双箭头
        /// </summary>
        DoubleArrow = 11,
        /// <summary>
        /// 斜箭头
        /// </summary>
        SlopeArrow = 12,

        ConnectionArrow = 13,
    }
}
