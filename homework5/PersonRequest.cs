class PersonRequest {
    private int fromFloor;
    private int toFloor;
    private int personId;
    private int elevatorId;
    private double timeStamp;
    public PersonRequest(int fromFloor, int toFloor, int personId, int elevatorId, double timeStamp) {
        this.fromFloor = fromFloor;
        this.toFloor = toFloor;
        this.personId = personId;
        this.elevatorId = elevatorId;
        this.timeStamp = timeStamp;
    }
    public int getFromFloor() {
        return fromFloor;
    }
    public int getToFloor() {
        return toFloor;
    }
    public int getPersonId() {
        return personId;
    }
    public int getElevatorId() {
        return elevatorId;
    }
    public double getTimeStamp() {
        return timeStamp;
    }
}