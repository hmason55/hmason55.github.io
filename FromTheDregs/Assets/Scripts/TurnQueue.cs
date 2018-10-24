using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnQueue {

	List<Turn> _queue;

	public List<Turn> queue {
		get {return _queue;}
	}

	public void RemoveTurns(BaseUnit baseUnit) {
		if(_queue == null) {return;}
		if(_queue.Count <= 0) {return;}

		for(int i = _queue.Count-1; i >= 0; i--) {
            Turn turn = _queue[i];

            if(turn != null) {
                if(turn.baseUnit != null) {
                    if(turn.baseUnit == baseUnit) {
                        _queue[i].baseUnit = null;
                    }
                }
            }
		}

	}

	public void NextTurn() {
		if(_queue == null) {return;}
		if(_queue.Count <= 0) {return;}
        
        for(int i = _queue.Count-1; i >= 0; i--) {
            Turn turn = _queue[i];
            
            // Remove null instances of turns from the queue.
            if(turn == null) {
                _queue.Remove(turn);
            } else if(turn.baseUnit == null) {
                _queue.Remove(turn);
            } else {
                // Older turns will have decreased priority and will eventually be moved to the front of the queue.
                turn.priority--;
            }
		}

		SortTurns();
	}

	void SortTurns() {
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

	void EndTurn() {
		_queue.RemoveAt(0);
		NextTurn();
	}

	void ClearTurns() {
		_queue = new List<Turn>();
	}


}
