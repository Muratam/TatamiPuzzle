public struct Point2D {
    public int x;
    public int y;

    public Point2D(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public int SquareLength {
        get { 
            return x * x + y * y;
        }
    }

    public static Point2D operator+(Point2D p1, Point2D p2) {
        return new Point2D(p1.x + p2.x, p1.y + p2.y);
    }
}
