public class Scout : Pheromone
{
    public string name = "Scout";
    public string target;

    public void Scout(string target)
    {
        this.target = target;
    }
    
    IEnumerator Start()
    {
        this.active = true;

        while (this.active)
        {
            // guess a destination and wander
            float guessRadius = 3;
            float xGuess = Random.Range(-guessRadius, guessRadius + 1);
            float zGuess = Random.Range(-guessRadius, guessRadius + 1);
            guess = new Vector3(xGuess, 0, zGuess);
            this.destination = transform.position + guess;
            float wanderTime = 0.5f;

            yield return new WaitForSeconds(wanderTime + Random.Range(0f, 0.1f));
        }
    }

    public void CheckActive(Collision collision)
    {
        // this pheromone should pass control to the next pheromone whenever the ant collides with the target
        if (collision.gameObject.name == "Curiosity(Clone)")
        {
            this.active = false;
            Debug.Log("Ant found a curiosity!");
        }

        return this.active;
    }
}