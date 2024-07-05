class PersonRequest {
    private int a; // RESET: fullPerson, FROMTO: fromFloor;
    private int b; // RESET: moveTime(ms), FROMTO: toFloor;
    private int c; // RESET: elevatorId/-1, FROMTO: personId;
    private int elevatorId;
    private double timeStamp;
    public PersonRequest(int type, int a, int b, int c, double timeStamp) {
        // type 0: RESET, 1: FROMTO, 2: DC RESET
        this.a = a;
        this.b = b;
        this.timeStamp = timeStamp;
        if(type==0) {
            // RESET
            this.c = -1;
            elevatorId = c;
        } else if(type==1) {
            // FROMTO
            this.c = c;
            elevatorId = 0; // no RECEIVE
        }
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