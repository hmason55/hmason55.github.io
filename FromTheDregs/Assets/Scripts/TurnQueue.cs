using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnQueue {

	List<Turn> _queue;

	public List<Turn> queue {
		get {return _queue;}
	}

    public TurnQueue() {
        _queue = new List<Turn>();
    }

    public int Length {
        get {
            if(_queue != null) {
                return _queue.Count;
            } else {
                return 0;
            }
        }
    }

    public void Add(Turn turn) {
        _queue.Add(turn);
    }

	public void RemoveTurns(BaseUnit baseUnit) {
		if(_queue == null) {return;}
		if(_queue.Count <= 0) {return;}

        // Remove instances of this unit from the queue.
		for(int i = _queue.Count-1; i >= 0; i--) {
            if(_queue[i].baseUnit == baseUnit) {
                _queue.RemoveAt(i);
            }
		}

	}

	public void NextTurn() {
		if(_queue == null) {return;}
		if(_queue.Count <= 0) {return;}
        
        for(int i = _queue.Count-1; i >= 0; i--) {
            Turn turn = _queue[i];
            
            // Older turns will have decreased priority and will eventually be moved to the front of the queue.
            turn.priority--;
            
		}

		SortTurns();
        
        _queue[0].baseUnit.BeginTurn();
	}

	public void SortTurns() {
		// Insertion sort based on turn priority. (ascending)
        if(_queue == null) {return;}
		if(_queue.Count < 2) {return;}
        
        int i = 1;
        while(i < _queue.Count) {
            int j = i;
            
            while(j > 0 && _queue[j - 1].priority > _queue[j].priority) {
                Turn temp = _queue[j];
                _queue[j] = _queue[j - 1];
                _queue[j - 1] = temp;
                j--;
            }
            i++;
		}
	}

    public bool UnitInQueue(BaseUnit b) {
        foreach(Turn t in _queue) {
            if(t.baseUnit == b) {
                return true;
            }
        }

        return false;
    }

	public void EndTurn() {
        if(_queue.Count > 0) {
		    _queue.RemoveAt(0);
        }
	}

    public void BeginCombat() {

    }

    public void EndCombat() {
        ClearTurns();
    }

	void ClearTurns() {
		_queue = new List<Turn>();
	}

    public void PrintTurns() {
        foreach(Turn turn in _queue) {
            Debug.Log(turn.baseUnit.spritePreset.ToString() + ", " + turn.priority);
        }
    }


}
