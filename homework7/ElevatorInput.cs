using System.Collections.Concurrent;

class ElevatorInput {
    private ConcurrentQueue<PersonRequest> requests;
    public ElevatorInput() {
        requests = new ConcurrentQueue<PersonRequest>();

        // PersonRequest request = new(2, 6, 400, 1, 5, 2.1);
        // requests.Add(request);
        // request = new(1, 1, 10, 2, -1, 5.6);
        // requests.Add(request);

        PersonRequest request = new(2, 8, 300, 1, 5, 1.4);
        requests.Enqueue(request);
        request = new(1, 2, 9, 1, -1, 4.1);
        requests.Enqueue(request);
        request = new(2, 8, 300, 2, 6, 5.3);
        requests.Enqueue(request);
        request = new(2, 6, 400, 6, 8, 8.9);
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