import { FC, useEffect, useState } from "react";

interface PokemonModel {
    id: number,
    name: string,
    weight: number,
    sprites: {
        front_default: string,
        back_default: string
    }
}

interface PokemonProps {
    id: number
}

const fetchPokemonById = async (id: number): Promise<any> => {
    const response = await fetch(`https://pokeapi.co/api/v2/pokemon/${id}`);
    if (!response.ok) {
        if (response.status === 404) {
          throw new Error('Pokémon not found');
        }
        throw new Error('Network response was not ok');
      }
    return response.json();
  };

const Pokemon: FC<PokemonProps> = ({id: propId}) => {
    const [id, setId] = useState(propId);
    const [pokemon, setPokemon] = useState<PokemonModel>();
    const [isFetching, setIsFetching] = useState<boolean>(false);
    const [error, setError] = useState<string>();

    const fetchPokemon = async() => {
        try {
            setIsFetching(true)
            setError(undefined);
            const result: any = await fetchPokemonById(id)
            setPokemon({
                id: result.id,
                name: result.name,
                weight: result.weight,
                sprites: {
                    front_default: result.sprites.front_default,
                    back_default: result.sprites.back_default
                }
            });
        } catch (error) {
            setError((error as Error).message);
        } finally {
            setIsFetching(false)
        }
    }
    
    useEffect(() => {
        fetchPokemon()
    }, [id])

    const renderInfo = () => {
        if (error) return (<div>Error: {error}<br/></div>)

        if (isFetching) return (<div>Loading...<br/></div>)
    
        if (!pokemon) return (<div>No data.<br/></div>)

        return (
            <>
                <div>Name: {pokemon.name}</div>
                <div>Weight: {pokemon.weight}</div>
                <div style={{display: 'flex'}}>
                    <img alt={`${pokemon.name}_front_default`} src={pokemon.sprites.front_default}></img>
                    <img alt={`${pokemon.name}_back_default`} src={pokemon.sprites.back_default}></img>
                </div>
            </>
        )
    }

    return (
        <div>
            <div>ID: {id}</div>
            {renderInfo()}
            {!isFetching && <div style={{display: 'flex'}}>
                <button onClick={() => setId(id - 1)}>Previous</button>
                <button onClick={() => setId(id + 1)}>Next</button>
            </div>}
        </div>
    )
}

export default Pokemon;