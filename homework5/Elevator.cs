using System;
using System.Collections.Generic;
class Elevator {
    private RequestQueue queue;
    private int curFloor;
    private int direction;
    public Elevator() {
        curFloor = 1;
        direction = 0;
        queue = new RequestQueue();
    }

    public void Add(PersonRequest pr) {
        queue.add(pr);
    }
    public PersonRequest? GetFirst() {
        return queue.getFirst();
    }
    public void SetStop(bool _stop) {
        queue.setStop(_stop);
    }
    public bool GetStop() {
        return queue.getStop();
    }
    public int GetDirection() {
        return direction;
    }
    public int GetCurFloor() {
        return curFloor;
    }
    public void run() {
        while (!(queue.IsEmpty() && queue.getStop())) {
            PersonRequest? pr = queue.getFirst();
            if (pr != null) {
                Thread.Sleep((int)(pr.getTimeStamp()*1000));
                var eq = new PriorityQueue<PersonRequest, PersonRequestComparator>();
                Move(pr, pr.getFromFloor());
                Open(pr);
                
                TimeSpan oTime = DateTime.Now.Subtract(Homework5.beginTime);
                Console.WriteLine("["+oTime.TotalSeconds+"]IN-"+pr.getPersonId()+"-"+curFloor+"-"+pr.getElevatorId());
                
                eq.Enqueue(pr, new PersonRequestComparator());
                ChangeDir(pr.getToFloor());
                PersonRequest? tmp;
                while ((tmp = queue.hasEqualFrom(pr)) != null) {
                    oTime = DateTime.Now.Subtract(Homework5.beginTime);
                    Console.WriteLine("["+oTime.TotalSeconds+"]IN-"+tmp.getPersonId()+"-"+curFloor+"-"+pr.getElevatorId());
                    eq.Enqueue(tmp, new PersonRequestComparator());
                }
                Close(pr);
                while (eq.Count != 0) {
                    tmp = eq.Dequeue();
                    Move(tmp, tmp.getToFloor());
                    Open(tmp);
                    oTime = DateTime.Now.Subtract(Homework5.beginTime);
                    Console.WriteLine("["+oTime.TotalSeconds+"]OUT-"+tmp.getPersonId()+"-"+curFloor+"-"+tmp.getElevatorId());
                    while (eq.Count!=0 && eq.Peek().getToFloor() == tmp.getToFloor()) {
                        oTime = DateTime.Now.Subtract(Homework5.beginTime);
                        Console.WriteLine("["+oTime.TotalSeconds+"]OUT-"+eq.Dequeue().getPersonId()+"-"+curFloor+"-"+eq.Peek().getElevatorId());
                    }
                    Close(tmp);
                }
            }
        }
    }

    public void ChangeDir(int floor) {
        if (floor > curFloor) {
            direction = 1;
        } else if (floor < curFloor) {
            direction = -1;
        } else {
            direction = 0;
        }
    }

    public void Move(PersonRequest pr, int floor) {
        ChangeDir(floor);
        // Thread.Sleep(500 * Math.Abs(floor - curFloor));
        if(floor < curFloor) {
            for(int i = curFloor-1; i>=floor; i--) {
                Thread.Sleep(400);
                TimeSpan oTime = DateTime.Now.Subtract(Homework5.beginTime);
                Console.WriteLine("["+oTime.TotalSeconds+"]ARRIVE-"+i+"-"+pr.getElevatorId());
            }
        } else {
            for(int i = curFloor+1; i<=floor; i++) {
                Thread.Sleep(400);
                TimeSpan oTime = DateTime.Now.Subtract(Homework5.beginTime);
                Console.WriteLine("["+oTime.TotalSeconds+"]ARRIVE-"+i+"-"+pr.getElevatorId());
            }            
        }
        curFloor = floor;
    }
    public void Open(PersonRequest pr) {
        Thread.Sleep(200);
        TimeSpan oTime = DateTime.Now.Subtract(Homework5.beginTime);
        Console.WriteLine("["+oTime.TotalSeconds+"]OPEN-"+curFloor+"-"+pr.getElevatorId());
    }
    public void Close(PersonRequest pr){
        Thread.Sleep(200);
        TimeSpan oTime = DateTime.Now.Subtract(Homework5.beginTime);
        Console.WriteLine("["+oTime.TotalSeconds+"]CLOSE-"+curFloor+"-"+pr.getElevatorId());
    }
    private class PersonRequestComparator : IComparer<PersonRequest> {
        public int Compare(PersonRequest? o1, PersonRequest? o2) {
            if(o1==null || o2==null) return 0;
            return o1.getToFloor() - o2.getToFloor();
        }
    }
}