class Schedule {
    private Elevator[] ele;
    private RequestQueue queue;
    public Schedule(RequestQueue queue) {
        this.queue = queue;
        ele = new Elevator[7];
        for (int i = 0; i < ele.Length; i++) {
            ele[i] = new Elevator(i);
            Thread t = new(ele[i].run);
            t.Start();
        }
    }

    public void eleAdd(int idx, PersonRequest pr) {
        ele[idx].add(pr);
    }

    public int reveive(PersonRequest pr) {
        int elevator = 0;
        int count_min = 127;
        for(int i=1; i<=6; i++) {
            if(ele[i].queue.getRequests().Count < count_min) {
                elevator = i;
                count_min = ele[i].queue.getRequests().Count;
                // break;
            }
        }
        TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
        string receive_string = "["+oTime.TotalSeconds+"]RECEIVE-"+pr.getPersonId();
        receive_string += "-"+elevator;
        Console.WriteLine(receive_string);
        return elevator;
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
                TimeSpan timeElapsed = DateTime.Now.Subtract(Homework6.beginTime);
                double timeDifference = pr.getTimeStamp() - timeElapsed.TotalSeconds;
                if(timeDifference <= 0) {
                    if(pr.getC() > 0) {
                        int elevator = reveive(pr);
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