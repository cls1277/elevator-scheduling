class ElevatorInput {
    private readonly List<PersonRequest> requests;
    private static int index;
    public ElevatorInput() {
        requests = [];

        // PersonRequest request = new(0, 8, 300, 1, 4.6);
        // requests.Add(request);
        // request = new(1, 5, 1, 2, 5.6);
        // requests.Add(request);
        // request = new(0, 5, 400, 1, 12.6);
        // requests.Add(request);

        // PersonRequest request = new(1, 2, 9, 1, 4.1);
        // requests.Add(request);
        // request = new(0, 8, 300, 3, 4.6);
        // requests.Add(request);
        // request = new(0, 8, 300, 2, 4.8);
        // requests.Add(request);
        // request = new(0, 8, 300, 1, 7.5);
        // requests.Add(request);
        // request = new(1, 1, 10, 2, 7.5);
        // requests.Add(request);
        // request = new(1, 3, 9, 3, 10);
        // requests.Add(request);

        PersonRequest request = new(0, 5, 400, 3, 1.1);
        requests.Add(request);
        request = new(1, 10, 6, 2, 1.9);
        requests.Add(request);
        request = new(0, 6, 300, 1, 5.1);
        requests.Add(request);
        request = new(1, 3, 5, 1, 6.6);
        requests.Add(request);
        request = new(0, 6, 400, 2, 8.3);
        requests.Add(request);
        request = new(0, 6, 400, 5, 9.0);
        requests.Add(request);
        request = new(0, 8, 300, 4, 10.0);
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