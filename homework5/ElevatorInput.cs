class ElevatorInput {
    private readonly List<PersonRequest> requests;
    private static int index;
    public ElevatorInput() {
        requests = [];
        // PersonRequest request = new(4, 5, 1, 1, 9.6);
        // requests.Add(request);

        PersonRequest request = new(2, 9, 1, 1, 4.1);
        requests.Add(request);
        request = new(1, 10, 2, 2, 7.5);
        requests.Add(request);

        // PersonRequest request = new(10, 6, 2, 1, 1.9);
        // requests.Add(request);
        // request = new(3, 5, 1, 2, 6.6);
        // requests.Add(request);


        // PersonRequest request = new(2, 8, 1, 1, 2.7);
        // requests.Add(request);
        // request = new(11, 3, 2, 2, 4.3);
        // requests.Add(request);
        // request = new(3, 8, 3, 3, 7.4);
        // requests.Add(request);
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