import { FC, useState } from 'react'
import Welcome from './components/Welcome'
import Counter from './components/Counter'
import Checkbox from './components/Checkbox'
import Pokemon from './components/Pokemon'
import ListComponent from './components/List'

const App: FC = () => {
  const [option, setOption] = useState("Welcome");

  const renderComponent = (option: string) => {
    switch (option) {
      case "Welcome":
        return <Welcome />
      case "Counter":
        return <Counter />
      case "Checkboxes":
        return <Checkbox />
      case "Pokemon":
        return <Pokemon id={1} />
      case "List":
        return <ListComponent />
      default:
        return null; // Handle default case
    }
  }

  return (
    <>
      <select name='option' onChange={(e) => setOption(e.target.value)} defaultValue={"Welcome"}>
        {["Welcome", "Counter", "Checkboxes", "Pokemon", "List"].map(item => {
          return (<option key={item} value={item}>{item}</option>)
        })}
      </select>
      <div>Option selected: {option}</div>
      {renderComponent(option)}
    </>
  )
}

export default App
