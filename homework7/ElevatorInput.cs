using System.Collections.Concurrent;

class ElevatorInput {
    private ConcurrentQueue<PersonRequest> requests;
    public ElevatorInput() {
        requests = new ConcurrentQueue<PersonRequest>();

        // PersonRequest request = new(2, 6, 400, 1, 5, 2.1);
        // requests.Enqueue(request);
        // request = new(1, 1, 10, 2, -1, 5.6);
        // requests.Enqueue(request);

        // PersonRequest request = new(2, 8, 300, 1, 5, 1.4);
        // requests.Enqueue(request);
        // request = new(1, 2, 9, 1, -1, 4.1);
        // requests.Enqueue(request);
        // request = new(2, 8, 300, 2, 6, 5.3);
        // requests.Enqueue(request);
        // request = new(2, 6, 400, 6, 8, 8.9);
        // requests.Enqueue(request);

        PersonRequest request = new(1, 10, 6, 2, -1, 1.9);
        requests.Enqueue(request);
        request = new(1, 11, 6, 3, -1, 5.6);
        requests.Enqueue(request);
        request = new(0, 6, 300, 1, -1, 8.5);
        requests.Enqueue(request);
        request = new(1, 3, 5, 1, -1, 14.6);
        requests.Enqueue(request);
        request = new(2, 6, 400, 1, 5, 15.5);
        requests.Enqueue(request);
        request = new(1, 1, 11, 9, -1, 16.6);
        requests.Enqueue(request);
    }
     public PersonRequest? nextPersonRequest() {
        if(requests.IsEmpty)
            return null;
        else {
            if(requests.TryDequeue(out PersonRequest? pr))
                return pr;
            return null;
        }
    }
}