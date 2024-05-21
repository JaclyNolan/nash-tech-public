import { useState } from "react";

const Counter = () => {
    const [count, setCount] = useState(10);

    const addCount = () => {
        setCount(count + 1);
    }
    const minusCount = () => {
        setCount(count - 1);
    }
    return (
        <div style={{display: 'flex', flexDirection: "row"}}>
            <button onClick={minusCount}>-</button>
            <h4>{count}</h4>
            <button onClick={addCount}>+</button>
        </div>
    )
}

export default Counter