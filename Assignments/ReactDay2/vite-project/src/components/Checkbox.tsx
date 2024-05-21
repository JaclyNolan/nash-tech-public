import { useEffect, useState } from "react"

const Checkbox = () => {
    const [coding, setCoding] = useState(true);
    const [music, setMusic] = useState(false);
    const [reading, setReading] = useState(true);
    const [all, setAll] = useState(false);

    useEffect(() => {
        if (coding && music && reading) setAll(true);
        else setAll(false);
    }, [coding, music, reading])

    const toggleAll = (checked: boolean) => {
        setCoding(checked);
        setMusic(checked);
        setReading(checked);
    }
    return (
        <div>
            <div>Choose your interests</div>
            <br/>
            <input 
                type="checkbox" 
                name="all" 
                checked={all}
                onChange={(e) => toggleAll(e.target.checked)}
            />
            <label htmlFor="all">All</label>
            <br/>
            <input 
                type="checkbox" 
                name="coding" 
                checked={coding}
                onChange={(e) => setCoding(e.target.checked)}
            />
            <label htmlFor="coding">Coding</label>
            <br/>
            <input 
                type="checkbox" 
                name="music" 
                checked={music}
                onChange={(e) => setMusic(e.target.checked)}
            />
            <label htmlFor="music">Music</label>
            <br/>
            <input 
                type="checkbox" 
                name="reading_books" 
                checked={reading}
                onChange={(e) => setReading(e.target.checked)}
            />
            <label htmlFor="reading_books">Reading books</label>
            <br/>
            <div>You selected:</div>
            <div>{`{"coding":${Boolean(coding)},"music":${Boolean(music)},"reading":${Boolean(reading)}}`}</div>
        </div>
    )
}

export default Checkbox