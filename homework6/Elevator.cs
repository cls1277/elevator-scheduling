using System;
using System.Collections.Generic;
class Elevator {
    private RequestQueue queue;
    private int curFloor;
    private int direction;
    public int count;
    private int fullPerson;
    private int moveTime;
    public Elevator() {
        curFloor = 1;
        direction = 0;
        queue = new RequestQueue();
        fullPerson = 6;
        moveTime = 400;
    }

    public void add(PersonRequest pr) {
        queue.add(pr);
    }
    public PersonRequest? getFirst() {
        return queue.getFirst();
    }
    public void setStop(bool _stop) {
        queue.setStop(_stop);
    }
    public bool getStop() {
        return queue.getStop();
    }
    public int getDirection() {
        return direction;
    }
    public int getCurFloor() {
        return curFloor;
    }
    public void setMoveTime(int moveTime) {
        this.moveTime = moveTime;
    }
    public int getMoveTime() {
        return moveTime;
    }
    public void setFullPerson(int fullPerson) {
        this.fullPerson = fullPerson;
    }
    public int getFullPerson() {
        return fullPerson;
    }
    public void run() {
        while (!(queue.IsEmpty() && queue.getStop())) {
            PersonRequest? pr = queue.getFirst();
            if (pr != null) {
                
                if(pr.getC() > 0) {
                    // Thread.Sleep((int)(pr.getTimeStamp()*1000));
                    var eq = new PriorityQueue<PersonRequest, PersonRequestComparator>();
                    Move(pr, pr.getFromFloor());
                    Open(pr);
                    
                    TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
                    Console.WriteLine("["+oTime.TotalSeconds+"]IN-"+pr.getPersonId()+"-"+curFloor+"-"+pr.getElevatorId());
                    count ++;

                    eq.Enqueue(pr, new PersonRequestComparator());
                    ChangeDir(pr.getToFloor());
                    while (count+1<=getFullPerson()) {
                        PersonRequest? tmp = queue.hasEqualFrom(pr);
                        if(tmp==null) break;
                        oTime = DateTime.Now.Subtract(Homework6.beginTime);
                        Console.WriteLine("["+oTime.TotalSeconds+"]IN-"+tmp.getPersonId()+"-"+curFloor+"-"+pr.getElevatorId());
                        eq.Enqueue(tmp, new PersonRequestComparator());
                        count ++;
                    }
                    Close(pr);
                    while (eq.Count != 0) {
                        PersonRequest? tmp = eq.Dequeue();
                        Move(tmp, tmp.getToFloor());
                        Open(tmp);
                        oTime = DateTime.Now.Subtract(Homework6.beginTime);
                        Console.WriteLine("["+oTime.TotalSeconds+"]OUT-"+tmp.getPersonId()+"-"+curFloor+"-"+tmp.getElevatorId());
                        count --;
                        while (eq.Count!=0 && eq.Peek().getToFloor() == tmp.getToFloor()) {
                            oTime = DateTime.Now.Subtract(Homework6.beginTime);
                            Console.WriteLine("["+oTime.TotalSeconds+"]OUT-"+eq.Dequeue().getPersonId()+"-"+curFloor+"-"+eq.Peek().getElevatorId());
                            count --;
                        }
                        Close(tmp);
                    }
                } else {

                    Reset(pr);
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
                Thread.Sleep(getMoveTime());
                TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
                Console.WriteLine("["+oTime.TotalSeconds+"]ARRIVE-"+i+"-"+pr.getElevatorId());
            }
        } else {
            for(int i = curFloor+1; i<=floor; i++) {
                Thread.Sleep(getMoveTime());
                TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
                Console.WriteLine("["+oTime.TotalSeconds+"]ARRIVE-"+i+"-"+pr.getElevatorId());
            }            
        }
        curFloor = floor;
    }
    public void Open(PersonRequest pr) {
        Thread.Sleep(200);
        TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
        Console.WriteLine("["+oTime.TotalSeconds+"]OPEN-"+curFloor+"-"+pr.getElevatorId());
    }
    public void Close(PersonRequest pr){
        Thread.Sleep(200);
        TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
        Console.WriteLine("["+oTime.TotalSeconds+"]CLOSE-"+curFloor+"-"+pr.getElevatorId());
    }
    public void Reset(PersonRequest pr) {
        TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
        Console.WriteLine("["+oTime.TotalSeconds+"]RESET_ACCEPT-"+pr.getElevatorId()+"-"+pr.getFullPerson()+"-"+pr.getMoveTime());
        if(queue.requests.Count > 0) {
            Open(pr);
            RequestQueue new_queue = new();
            foreach(PersonRequest request in queue.requests) {
                oTime = DateTime.Now.Subtract(Homework6.beginTime);
                Console.WriteLine("["+oTime.TotalSeconds+"]OUT-"+request.getPersonId()+"-"+curFloor+"-"+request.getElevatorId());
                new_queue.add(new PersonRequest(1, curFloor, request.getToFloor(), request.getPersonId(), 0));
            }
            queue = new_queue;
            Close(pr);
        }
        oTime = DateTime.Now.Subtract(Homework6.beginTime);
        Console.WriteLine("["+oTime.TotalSeconds+"]RESET_BEGIN-"+pr.getElevatorId());
        Thread.Sleep(1200);
        setMoveTime(pr.getMoveTime());
        setFullPerson(pr.getFullPerson());
        oTime = DateTime.Now.Subtract(Homework6.beginTime);
        Console.WriteLine("["+oTime.TotalSeconds+"]RESET_END-"+pr.getElevatorId());
    }
    private class PersonRequestComparator : IComparer<PersonRequest> {
        public int Compare(PersonRequest? o1, PersonRequest? o2) {
            if(o1==null || o2==null) return 0;
            return o1.getToFloor() - o2.getToFloor();
        }
    }
}