class ElevatorInput {
    private readonly List<PersonRequest> requests;
    private static int index;
    public ElevatorInput() {
        requests = [];

        // PersonRequest request = new(2, 6, 400, 1, 5, 2.1);
        // requests.Add(request);
        // request = new(1, 1, 10, 2, -1, 5.6);
        // requests.Add(request);

        PersonRequest request = new(2, 8, 300, 1, 5, 1.4);
        requests.Add(request);
        request = new(1, 2, 9, 1, -1, 4.1);
        requests.Add(request);
        request = new(2, 8, 300, 2, 6, 5.3);
        requests.Add(request);
        request = new(2, 6, 400, 6, 8, 8.9);
        requests.Add(request);

        index = 0;
    }
    public PersonRequest? nextPersonRequest() {
        if(index == requests.Count) {
            return null;
        } else {
            PersonRequest pr = requests[index];
            index++;            
            return pr;
        }
    }
}