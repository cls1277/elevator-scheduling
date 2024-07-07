class Schedule {
    private Elevator[] ele;
    private RequestQueue queue;
    public Schedule(RequestQueue queue) {
        this.queue = queue;
        ele = new Elevator[7];
        for (int i = 0; i < ele.Length; i++) {
            ele[i] = new();
            Thread t = new(ele[i].run);
            t.Start();
        }
    }

    public void run() {
        bool flag = false;
        PersonRequest? pr = null;
        while (true) {
            if(queue.IsEmpty() && queue.getStop() && pr==null) {
                bool flag2 = false;
                for(int i=1; i<=6; i++) {
                    if(ele[i].getPr() != null) {
                        flag2 = true;
                    }
                }
                if(flag2 == false)
                    break;
            }
            if(flag == false)
                pr = queue.getFirst();
            if(pr != null) {
                TimeSpan timeElapsed = DateTime.Now.Subtract(Homework5.beginTime);
                double timeDifference = pr.getTimeStamp() - timeElapsed.TotalSeconds;
                if(timeDifference <= 0) {
                    ele[pr.getElevatorId()].add(pr);
                    flag = false;
                    pr = null;
                } else {
                    int sleepTime = (int)(timeDifference * 1000);
                    Thread.Sleep(sleepTime);
                    flag = true;
                }
            }
        }
        for (int i = 0; i < ele.Length; i++) {
            ele[i].setStop(true);
        }
    }
}