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
        bool flag = false;
        PersonRequest? pr = null;
        while (true) {
            if (queue.IsEmpty() && queue.getStop() && pr==null)
                break;
            if (flag == false)
                pr = queue.getFirst();
            if (pr != null) {
                // Thread.Sleep((int)(pr.getTimeStamp()*1000));
                TimeSpan timeElapsed = DateTime.Now.Subtract(Homework6.beginTime);
                double timeDifference = pr.getTimeStamp() - timeElapsed.TotalSeconds;
                if(timeDifference <= 0) {
                    if(pr.getC() > 0) {
                        int elevator = new Random().Next(1, 7);
                        TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
                        Console.WriteLine("["+oTime.TotalSeconds+"]RECEIVE-"+pr.getPersonId()+"-"+elevator);
                        pr.setElevatorId(elevator);
                        ele[pr.getElevatorId()].add(pr);
                        PersonRequest? tmp;
                        while ((tmp = queue.hasEqualFrom(pr)) != null) {
                            tmp.setElevatorId(pr.getElevatorId());
                            ele[pr.getElevatorId()].add(tmp);
                        }
                    } else {
                        ele[pr.getElevatorId()].add(pr);
                    }
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