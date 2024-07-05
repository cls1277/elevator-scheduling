class Input {
    private RequestQueue queue;
    public Input(RequestQueue queue) {
        this.queue = queue;
    }
    public void run() {
        ElevatorInput elevatorInput = new();
        while(true) {
            PersonRequest? request = elevatorInput.nextPersonRequest();
            if(request == null) {
                queue.setStop(true);
                break;
            } else {
                queue.add(request);
            }
        }
    }
}