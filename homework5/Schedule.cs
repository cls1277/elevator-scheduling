using System.Threading;

class Schedule {
    private Elevator[] ele;
    private RequestQueue queue;
    public Schedule(RequestQueue queue) {
        this.queue = queue;
        ele = new Elevator[7];
        for (int i = 0; i < ele.Length; i++) {
            ele[i] = new Elevator();
            Thread t = new(ele[i].run);
            t.Start();
        }
    }

    public void run() {
        while (true) {
            if (queue.IsEmpty() && queue.getStop())
                break;
            PersonRequest? pr = queue.getFirst();
            if (pr != null) {
                ele[pr.getElevatorId()].Add(pr);
                PersonRequest? tmp;
                while ((tmp = queue.hasEqualFrom(pr)) != null && pr.getElevatorId() == tmp.getElevatorId()) {
                    ele[pr.getElevatorId()].Add(tmp);
                }
            }
        }
        for (int i = 0; i < ele.Length; i++) {
            ele[i].SetStop(true);
        }
    }
}