import { useState } from 'react'
import Welcome from './components/Welcome'
import Counter from './components/Counter'
import Checkbox from './components/Checkbox'

function App() {
  const [option, setOption] = useState("Welcome");

  const renderComponent = (option: string) => {
    switch (option) {
      case "Welcome":
        return <Welcome/>
      case "Counter":
        return <Counter/>
      case "Checkboxes":
        return <Checkbox/>
      default:
        return null; // Handle default case
    }
  }

  return (
    <>
      <select name='option' onChange={(e) => setOption(e.target.value)}>
        <option value="Welcome" selected>Welcome</option>
        <option value="Counter">Counter</option>
        <option value="Checkboxes">Checkboxes</option>
      </select>

      {renderComponent(option)}
    </>
  )
}

export default App
