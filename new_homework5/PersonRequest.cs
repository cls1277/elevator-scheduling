class PersonRequest {
    private int a; // RESET: fullPerson, FROMTO: fromFloor;
    private int b; // RESET: moveTime(ms), FROMTO: toFloor;
    private int c; // RESET: elevatorId/-1, FROMTO: personId;
    private int elevatorId;
    private double timeStamp;
    public PersonRequest(int a, int b, int c, int d, double timeStamp) {
        this.a = a;
        this.b = b;
        this.c = c;
        elevatorId = d;
        this.timeStamp = timeStamp;
    }
    public int getFullPerson() { return a; }
    public int getMoveTime() { return b; }
    public int getFromFloor() { return a; }
    public int getToFloor() { return b; }
    public int getPersonId() { return c; }
    public int getElevatorId() { return elevatorId; }
    public void setElevatorId(int elevatorId) { this.elevatorId = elevatorId; }
    public double getTimeStamp() { return timeStamp; }
    public int getC() { return c; }
    public override bool Equals(object? obj) {
        if (obj == null || GetType() != obj.GetType())
            return false;
        PersonRequest other = (PersonRequest)obj;
        return a == other.a && b == other.b && c == other.c && elevatorId == other.elevatorId && timeStamp == other.timeStamp;
    }

    public override int GetHashCode() {
        return HashCode.Combine(a, b, c, elevatorId, timeStamp);
    }
}